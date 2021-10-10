using HttpRequest.Adapter;
using HttpRequest.Model;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace HttpRequest.Controllers
{
    /*
    https://stackoverflow.com/questions/36425008/mocking-httpclient-in-unit-tests
    */
    public class HttpRequestControllerTests
    {
        private readonly HttpRequestController _controller;
        private readonly Mock<IHttpClient> _mockHttpClient;

        private readonly PostCollection _postCollection;
        public HttpRequestControllerTests()
        {
            _mockHttpClient = new Mock<IHttpClient>();
            _controller = new HttpRequestController(_mockHttpClient.Object);
            var httpRequestMessage = new HttpRequestMessage();
            _mockHttpClient.Setup(x => x.DefaultRequestHeaders).Returns(httpRequestMessage.Headers);

            _postCollection = new PostCollection();

            var httpResponseMessage = 
            _mockHttpClient.Setup(x => x.GetAsync(HttpRequestController.Path))
                .Returns(() => Task.FromResult(
                    new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                        {
                            Content = new StringContent(JsonSerializer.Serialize(_postCollection))
                        }
                )
            );
        }

        [Fact]
        public void should_set_the_correct_base_address_in_the_http_client()
        {
            // arrange
            Uri capturedUri = null;
            _mockHttpClient.SetupSet(x => x.BaseAddress = It.IsAny<Uri>()).Callback<Uri>(x => capturedUri = x);

            // act
            _controller.Get();

            // assert
            Assert.Equal(HttpRequestController.BaseAddress, capturedUri.ToString());
        }

        [Fact]
        public void should_return_post_collection_from_api()
        {
            // arrange
            _postCollection.Data = new Post[] { new Post { body = "a", id = 1, title = "b", user_id = 2 } };

            // act
            var response =_controller.Get();

            // assert
            Assert.Equal(JsonSerializer.Serialize(_postCollection), JsonSerializer.Serialize(response));
        }
    }
}
