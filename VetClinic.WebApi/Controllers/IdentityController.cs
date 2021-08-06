using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace VetClinic.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public IdentityController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("callapi")]
        public async Task<IActionResult> CallApi()
        {

            var serverClient = _httpClientFactory.CreateClient();
            var disco = await serverClient.GetDiscoveryDocumentAsync("https://localhost:5101");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    
                    ClientId = "swagger",
                    ClientSecret = "secret",

                    Scope = "api1"

                });

            var apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient.GetAsync("https://localhost:44350");

            var content = await response.Content.ReadAsStringAsync();

            return Ok(new
            {
                access_token = tokenResponse.AccessToken,
                message = content
            });
        }
    }
}
