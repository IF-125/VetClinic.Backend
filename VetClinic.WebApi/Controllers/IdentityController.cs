using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        private static readonly HttpClient HttpClient = new HttpClient();

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


        //[Route("/callapi")]
        [HttpGet("callapi")]
        public async Task<IActionResult> CallApi()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5101/connect/token");

            
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            HttpResponseMessage response = await HttpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return Content(response.ToString());
            }

            return Content($"{await response.Content.ReadAsStringAsync()}");
        }
    }
}
