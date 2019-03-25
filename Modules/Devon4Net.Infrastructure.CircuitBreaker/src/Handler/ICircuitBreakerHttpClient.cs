namespace Devon4Net.Infrastructure.CircuitBreaker.Handler
{
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface ICircuitBreakerHttpClient
    {
        Task<string> DeleteAsync(string endPointName, string url);
        Task<Stream> GetAsStreamAsync(string endPointName, string url);
        Task<string> GetAsync(string endPointName, string url);
        Task<HttpResponseMessage> PatchAsync(string endPointName, string url, HttpContent content);
        Task<T> PostAsync<T>(string endPointName, string url, object dataToSend, string mediaType);
        Task<T> PutAsync<T>(string endPointName, string url, object dataToSend, string mediaType);
    }
}