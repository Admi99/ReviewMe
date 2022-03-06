using System.Net.Http.Json;

namespace ReviewMe.Core.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient();
    }

    public async Task<T?> GetFromJsonAsync<T>(string requestUri)
        => await _httpClient.GetFromJsonAsync<T>(requestUri);
}