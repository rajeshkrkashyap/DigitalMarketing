using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : ControllerBase
    {

        [HttpGet("Send")]
        public HttpResponseMessage Send(string mobileNumber, string countryCode, string message)
        {
            return SMSLibrary.SmsServiceProvider.SendSMS(mobileNumber, countryCode, message);
        }
    }
}
