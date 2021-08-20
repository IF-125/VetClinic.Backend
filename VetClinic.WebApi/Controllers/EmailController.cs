using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VetClinic.Core.Interfaces.Services;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult Send(EmailViewModel model)
        {
            _emailService.Send(model.From, model.To, model.Subject, model.Body);
            return Ok();
        }
    }
}
