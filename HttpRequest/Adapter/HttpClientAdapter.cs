using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpRequest.Adapter
{
    public sealed class HttpClientAdapter : IHttpClient
    {
        HttpClient httpClient;
        
        public HttpClientAdapter()
        {
            httpClient = new HttpClient();
        }

        public Uri BaseAddress { get => httpClient.BaseAddress; set => httpClient.BaseAddress = value; }

        public HttpRequestHeaders DefaultRequestHeaders { get => httpClient.DefaultRequestHeaders; }

        public Task<HttpResponseMessage> GetAsync(string path)
        {
            return httpClient.GetAsync(path);
        }
    }
}
