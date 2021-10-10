using HttpRequest.Adapter;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpRequest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpRequestController : ControllerBase
    {
        public const string BaseAddress = "https://gorest.co.in/";
        public const string Path = "public/v1/users/123/posts";

        private readonly IHttpClient _httpClient;
        
        public HttpRequestController(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public Model.PostCollection Get()
        {
            return GetDetails().Result;
        }

        private async Task<Model.PostCollection> GetDetails()
        {
            /*
            HttpClient is intended to be instantiated once and reused throughout the life of an application
            https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client#create-and-initialize-httpclient
            */

            _httpClient.BaseAddress = new Uri(BaseAddress);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            HttpResponseMessage response = await _httpClient.GetAsync(Path);
            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Model.PostCollection>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;

        }
    }
}
