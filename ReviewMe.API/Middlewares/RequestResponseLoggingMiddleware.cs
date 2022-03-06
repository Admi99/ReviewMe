using Microsoft.IO;

namespace ReviewMe.API.Middlewares;

public sealed class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
    private const int ReadChunkBufferLength = 4096;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            await LogRequestAsync(context.Request);
            await LogResponseAsync(context);
        }
        else
        {
            await _next.Invoke(context);
        }
    }

    private async Task LogRequestAsync(HttpRequest request)
    {
        request.EnableBuffering();

        using var requestStream = _recyclableMemoryStreamManager.GetStream();
        await request.Body.CopyToAsync(requestStream);
        var requestBody = ReadStreamInChunks(requestStream);

        _logger.LogDebug(
            "Http Request Information: Schema:'{@requestScheme}' Host:'{@requestHost}' Path:'{@requestPath}' QueryString:'{@requestQueryString}' Request Body:'{@requestBody}'",
            request.Scheme,
            request.Host,
            request.Path,
            request.QueryString,
            requestBody
        );

        request.Body.Position = 0;
    }

    private async Task LogResponseAsync(HttpContext context)
    {

        var originalBodyStream = context.Response.Body;

        await using var responseBody = _recyclableMemoryStreamManager.GetStream();
        context.Response.Body = responseBody;

        await _next(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.LogDebug(
            "Http Response Information: Schema:'{@requestScheme}' Host:'{@requestHost}' Path:'{@requestPath}' QueryString:'{@requestQueryString}' Response Body:'{@responseBody}'",
            context.Request.Scheme,
            context.Request.Host,
            context.Request.Path,
            context.Request.QueryString,
            text
        );

        await responseBody.CopyToAsync(originalBodyStream);

    }

    private static string ReadStreamInChunks(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        string result;
        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream);
        var readChunk = new char[ReadChunkBufferLength];

        int readChunkLength;
        //do while: is useful for the last iteration in case readChunkLength < chunkLength
        do
        {
            readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
            textWriter.Write(readChunk, 0, readChunkLength);
        } while (readChunkLength > 0);

        result = textWriter.ToString();

        return result;
    }
}