namespace ReviewMe.Core.Services;

public interface IHttpClientService
{
    Task<T?> GetFromJsonAsync<T>(string requestUri);
}