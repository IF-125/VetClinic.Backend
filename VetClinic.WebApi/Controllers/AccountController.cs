using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.ViewModels.AuthViewModels;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJWTTokenGenerator _token;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IJWTTokenGenerator token)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
        }
  
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            User user = new User { Email = model.Email, UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                
            var result = await _userManager.CreateAsync(user, model.Password);
            await _signInManager.SignInAsync(user, false);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Role", "Client"));

            return Ok();
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _signInManager.PasswordSignInAsync(model.Email, 
                                                                  model.Password, 
                                                                  model.RememberMe, 
                                                                  false);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok(new
            {
                result = result,
                email = model.Email,
                token = _token.GenerateToken(user)
            });
    }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
