using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
  
        [HttpPost]
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

        [Route("Login")]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var res = returnUrl;
            return Content(res);
        }
        
        [HttpPost("validate")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _signInManager.PasswordSignInAsync(model.Email,
                                                                      model.Password,
                                                                      model.RememberMe,
                                                                      false);
            if (result.Succeeded)
            {
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
            }
            else
            {
                ModelState.AddModelError("", "Wrong login or password");
            }
            
            return Ok(model);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("api/Home");
        }
    }
}
