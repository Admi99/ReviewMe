using ReviewMe.Core.Exceptions;
using System.Net;
using System.Security.Authentication;

namespace ReviewMe.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ErrorTypeException exception)
        {
            var response = ErrorResponse.Create(exception.ErrorType, exception.Message);
            _logger.LogError(exception, "There was an " + nameof(ErrorTypeException) + ". {@errorResponse}", response);

            await WriteJsonErrorAsync(context, GetHttpStatusCode(exception.ErrorType), response.ErrorMessage);
        }
        catch (AuthenticationException exception)
        {
            //All handled exceptions from the system will be mostly of type ErrorTypeException(ErrorType.Authentication)
            //This exception is expected to be thrown only by the framework

            var result = ErrorResponse.Create(ErrorType.Authentication, exception.Message);
            _logger.LogWarning(exception,
                "There was an " + nameof(AuthenticationException) + " thrown from the system. {@errorResponse}",
                result);

            await WriteJsonErrorAsync(context, HttpStatusCode.Unauthorized, result);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex, "There was an " + nameof(OperationCanceledException) + " thrown from the system.");

            //DO NOT return WriteJsonErrorAsync - the response thread is canceled already!
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "There was an unexpected unhandled exception. Must be fixed in the source code!");

            //In PROD environment we don't want to send any FileNotFound/Authentication detail/ ... Exceptions to FE. It is a security risk!
            //See ErrorResponse.Message property marked by JsonIgnoreAttribute for PROD
            await WriteJsonErrorAsync(context, HttpStatusCode.InternalServerError);
        }
    }

    private static HttpStatusCode GetHttpStatusCode(ErrorType errorType)
        => errorType switch
        {
            ErrorType.GeneralRequestValidation => HttpStatusCode.BadRequest,
            ErrorType.Authentication => HttpStatusCode.Unauthorized,
            ErrorType.Authorization => HttpStatusCode.Forbidden,
            ErrorType.ResourceNotFound => HttpStatusCode.NotFound,
            ErrorType.GenericServerError => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError
        };

    private static Task WriteJsonErrorAsync(HttpContext context, HttpStatusCode code)
        => WriteJsonErrorAsync(context, code, null);

    private static Task WriteJsonErrorAsync(HttpContext context, HttpStatusCode code, object? result)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsJsonAsync(result);
    }
}