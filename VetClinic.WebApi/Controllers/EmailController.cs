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
            string from = "user@email.com";
            string to = "administrator@email.com";
            string subject = "Request for an appointment.";
            string body = "User wants to request " + model.Procedure + " for their pet " + model.Pet;
            _emailService.Send(from, to, subject, body);
            return Ok();
        }
    }
}
