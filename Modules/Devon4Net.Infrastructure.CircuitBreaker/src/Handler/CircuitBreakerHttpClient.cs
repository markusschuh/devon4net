using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Devon4Net.Infrastructure.CircuitBreaker.Handler
{
    public class CircuitBreakerHttpClient : ICircuitBreakerHttpClient
    {
        private IHttpClientFactory HttpClientFactory { get; set; }
        private ILogger Logger { get; set; }
        public CircuitBreakerHttpClient(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
            Logger = null;
        }

        public CircuitBreakerHttpClient(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            HttpClientFactory = httpClientFactory;
            Logger = logger;
        }

        public async Task<string> Get(string endPointName, string url)
        {
            HttpClient httpClient = null;
            HttpResponseMessage httpResponseMessage = null;
            var result = string.Empty;

            try
            {
                var errorHttp = false;
                using (httpClient = GetDefaultClient(endPointName))
                {
                    httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + url).ConfigureAwait(false);

                    if (httpResponseMessage != null && httpResponseMessage.IsSuccessStatusCode)
                    {
                        result = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        errorHttp = true;
                    }

                    httpClient = null;
                    httpResponseMessage = null;

                    if (errorHttp) throw new HttpRequestException($"The httprequest to {endPointName} was not successful.");
                }
            }
            catch (Exception ex)
            {
                LogException(ref ex);
                throw;
            }
            finally
            {
                if (httpClient != null) httpClient.Dispose();
                if (httpResponseMessage != null) httpResponseMessage.Dispose();
            }

            return result;
        }

        public async Task<Stream> GetAsStream(string endPointName, string url)
        {
            Stream result = null;
            HttpClient httpClient = null;
            HttpResponseMessage httpResponseMessage = null;
            try
            {
                var errorHttp = false;
                using (httpClient = GetDefaultClient(endPointName))
                {
                    httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + url).ConfigureAwait(false);

                    if (httpResponseMessage?.IsSuccessStatusCode == true)
                    {
                        result = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        errorHttp = true;
                    }

                    httpClient = null;
                    httpResponseMessage = null;

                    if (errorHttp) throw new HttpRequestException($"The httprequest to {endPointName} was not successful.");
                }
            }
            catch (Exception ex)
            {
                LogException(ref ex);
                throw;
            }
            finally
            {
                if (httpClient != null) httpClient.Dispose();
                if (httpResponseMessage != null) httpResponseMessage.Dispose();
            }

            return result;
        }

        public async Task<T> Post<T>(string endPointName, string url, object dataToSend, string mediaType)
        {
            T result;
            var httpResult = string.Empty;
            HttpClient httpClient = null;
            HttpContent httpContent = null;
            HttpResponseMessage httpResponseMessage = null;

            try
            {
                var errorHttp = false;
                using (httpClient = GetDefaultClient(endPointName))
                {
                    httpContent = CreateJsonHttpContent(dataToSend, mediaType);

                    httpResponseMessage = await httpClient.PostAsync(httpClient.BaseAddress + url, httpContent).ConfigureAwait(false);

                    if (httpResponseMessage?.IsSuccessStatusCode == true)
                    {
                        httpResult = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        errorHttp = true;
                    }

                    httpClient = null;
                    httpResponseMessage = null;
                    httpContent = null;

                    if (errorHttp) throw new HttpRequestException($"The httprequest to {endPointName} was not successful.");

                    result = Deserialize<T>(httpResult);
                }
            }
            catch (Exception ex)
            {
                LogException(ref ex);
                throw;
            }
            finally
            {
                if (httpClient != null) httpClient.Dispose();
                if (httpContent != null) httpContent.Dispose();
                if (httpResponseMessage != null) httpResponseMessage.Dispose();
            }

            return result;
        }

        public async Task<T> Put<T>(string endPointName, string url, object dataToSend, string mediaType)
        {
            T result;
            var httpResult = string.Empty;
            HttpClient httpClient = null;
            HttpContent httpContent = null;
            HttpResponseMessage httpResponseMessage = null;

            try
            {
                var errorHttp = false;
                using (httpClient = GetDefaultClient(endPointName))
                {
                    httpContent = CreateJsonHttpContent(dataToSend, mediaType);
                    httpResponseMessage = await httpClient.PutAsync(httpClient.BaseAddress + url, httpContent).ConfigureAwait(false);

                    if (httpResponseMessage?.IsSuccessStatusCode == true)
                    {
                        httpResult = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        errorHttp = true;
                    }

                    httpClient = null;
                    httpResponseMessage = null;
                    httpContent = null;

                    if (errorHttp) throw new HttpRequestException($"The httprequest to {endPointName} was not successful.");

                    result = Deserialize<T>(httpResult);
                }
            }
            catch (Exception ex)
            {
                LogException(ref ex);
                throw;
            }
            finally
            {
                if (httpClient != null) httpClient.Dispose();
                if (httpContent != null) httpContent.Dispose();
                if (httpResponseMessage != null) httpResponseMessage.Dispose();
            }

            return result;

        }

        public async Task<string> Delete(string endPointName, string url)
        {
            HttpClient httpClient = null;
            HttpResponseMessage httpResponseMessage = null;
            var result = string.Empty;

            try
            {
                var errorHttp = false;
                using (httpClient = GetDefaultClient(endPointName))
                {
                    httpResponseMessage = await httpClient.DeleteAsync(httpClient.BaseAddress + url).ConfigureAwait(false);

                    if (httpResponseMessage?.IsSuccessStatusCode == true)
                    {
                        result = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        errorHttp = true;
                    }

                    httpClient = null;
                    httpResponseMessage = null;

                    if (errorHttp) throw new HttpRequestException($"The httprequest to {endPointName} was not successful.");
                }
            }
            catch (Exception ex)
            {
                LogException(ref ex);
                throw;
            }
            finally
            {
                if (httpClient != null) httpClient.Dispose();
                if (httpResponseMessage != null) httpResponseMessage.Dispose();
            }

            return result;
        }

        public async Task<HttpResponseMessage> Patch(string endPointName, string url, HttpContent content)
        {
            HttpClient httpClient = null;
            HttpResponseMessage result = null;
            try
            {
                var method = new HttpMethod("PATCH");

                using (httpClient = GetDefaultClient(endPointName))
                {
                    var request = new HttpRequestMessage(method, httpClient.BaseAddress + url)
                    {
                        Content = content
                    };

                    result = await httpClient.SendAsync(request).ConfigureAwait(false);

                    httpClient = null;
                }
            }
            catch (Exception ex)
            {
                LogException(ref ex);
                throw;
            }
            finally
            {
                if (httpClient != null) httpClient.Dispose();
            }

            return result;
        }

        private HttpClient GetDefaultClient(string endPointName)
        {
            return HttpClientFactory.CreateClient(endPointName);
        }

        private HttpContent CreateJsonHttpContent<T>(T requestContent, string mediaType)
        {
            var requestBody = Serialize(requestContent);
            HttpContent httpContent = new StringContent(requestBody);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return httpContent;
        }

        private void LogErrorMessage(string message)
        {
            Console.WriteLine(message);

            if (Logger != null)
            {
                Logger.LogError(message);
            }
        }

        private void LogException(ref Exception exception)
        {
            LogErrorMessage($"{exception.Message} : {exception.InnerException}");
        }

        private string Serialize(object toPrint)
        {
            return JsonSerializer.Serialize(toPrint);
        }

        private T Deserialize<T>(string input)
        {
            return JsonSerializer.Deserialize<T>(input);
        }
    }
}