using Microsoft.AspNetCore.Mvc;

namespace VetClinic.Backend.Controllers
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
    }
}
