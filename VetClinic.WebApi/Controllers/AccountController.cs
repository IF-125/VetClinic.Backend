using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Services;
using VetClinic.DAL.Context;
using VetClinic.WebApi.ViewModels.AuthViewModels;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTTokenGenerator _token;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJWTTokenGenerator token)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
            var role = await _roleManager.FindByNameAsync("Client");
            await _userManager.AddToRoleAsync(user, "Client");
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

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles[0];
            var token = _token.GenerateToken(user, role);

            return Ok(new AuthResponseViewModel {Token = token });
    }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if(role == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            return Ok();
        }
    }
}
