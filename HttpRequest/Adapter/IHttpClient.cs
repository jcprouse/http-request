using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpRequest.Adapter
{
    public interface IHttpClient
    {
        public Uri BaseAddress { get; set; }

        public HttpRequestHeaders DefaultRequestHeaders { get; }

        public Task<HttpResponseMessage> GetAsync(string path);
    }
}
