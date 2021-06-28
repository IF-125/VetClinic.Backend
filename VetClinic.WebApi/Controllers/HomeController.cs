﻿using Microsoft.AspNetCore.Mvc;

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
    }
}