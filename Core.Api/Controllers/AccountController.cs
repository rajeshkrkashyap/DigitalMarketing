using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="admin")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet(), HttpPost()]
        [Route("GetDetail")]
        public IEnumerable<Account> Get()
        {
            //TODO Write a logic to get user account detail  
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Account
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
