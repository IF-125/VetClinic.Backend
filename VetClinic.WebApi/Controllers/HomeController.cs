using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHello()
        {
            return Ok("Everyone gangsta till project initialization");
        }

        [HttpGet("{id}")]
        public IActionResult GetHelloById(int id)
        {
            return Ok($"You got nothing for {id}");
        }

        //[HttpGet]
        //public async Task<IActionResult> CallApi()
        //{
        //    var accessToken = await HttpContext.GetTokenAsync("access_token");

        //    var client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //    var content = await client.GetStringAsync("https://localhost:5101/connect/token");
        
        //    return Ok(content);
        //}
    }
}
