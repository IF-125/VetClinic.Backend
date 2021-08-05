using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            string refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            return Content($"Current user: <span id=\"UserIdentityName\">{User.Identity.Name ?? "anonymous"}</span><br/>" +
                $"<div>Access token: {accessToken}</div><br/>" +
                $"<div>Refresh token: {refreshToken}</div><br/>"
                , "text/html");
        }


        //[Route("/")]
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


            //string accessToken = await HttpContext.GetTokenAsync("access_token");
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5101/connect/token");


            //request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            //HttpResponseMessage response = await HttpClient.SendAsync(request);

            //if (response.StatusCode != HttpStatusCode.OK)
            //{
            //    return Content(response.ToString());
            //}

            //return Content($"{await response.Content.ReadAsStringAsync()}");
        }
    }
}
