using Azure.Core;
using ConnectToAi.Models;
using ConnectToAi.Services;
using ConnectToAi.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Shared;
using Newtonsoft.Json;
using Stripe.BillingPortal;
using System.Diagnostics;

namespace ConnectToAi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthService _authService;
        private readonly ConfigService _configService;
        public HomeController(ILogger<HomeController> logger, AuthService authService, ConfigService configService)
        {
            _logger = logger;
            _authService = authService;
            _configService = configService;
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("ConnectToAi_DigitalMarketing_AuthToken");
            ViewBag.IsLogOut = true;
            return View(nameof(Index));
        }

        public IActionResult Default()
        {
            return View();
        }
        public async Task<JsonResult> SendOTP(string countryCode, string mobileNumber)
        {
            string sessionId = HttpContext.Session.Id;

            var response = await _authService.SendOTP(mobileNumber, countryCode);

            return Json(sessionId);
        }
        public IActionResult Index()
        {
            Request.Cookies.TryGetValue("ConnectToAi_DigitalMarketing_AuthToken", out string? cookieValue);
            if (cookieValue != null)
            {
                var userDetail = JsonConvert.DeserializeObject<UserDetail?>(cookieValue);
                ViewBag.Role = userDetail.Role;
                return AutoRedirect(userDetail.AccessToken);
            }

            return View();
        }
        public IActionResult AutoRedirect(string accessToken = null)
        {
            HttpContext.Request.Cookies.TryGetValue("ConnectToAi_DigitalMarketing_AuthToken", out string cookieValue);
            var host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;

            if (string.IsNullOrEmpty(cookieValue))
            {
                if (!string.IsNullOrEmpty(accessToken))
                {
                    LocalRedirectFuntion(accessToken, host);
                }
                var redirectUrl = host + "/Identity/Account/LoginApp";
                HttpContext.Response.Redirect(redirectUrl);
            }
            else
            {
                LocalRedirectFuntion(cookieValue, host);
            }
            return View();
        }

        private void LocalRedirectFuntion(string cookieValue, string host)
        {
            if (cookieValue == null || host == null)
            {
                return;
            }

            UserDetail userDetail = JsonConvert.DeserializeObject<UserDetail>(cookieValue);
            switch (userDetail.Role.ToLower())
            {
                case "marketing":
                    using (ProjectService projectService = new(_configService))
                    {
                        var projects = projectService.ListAsync(userDetail.UserID).Result;
                        if (projects.Count() > 0)
                        { ViewBag.RedirectUrl = host + "/marketing/Dashboard/Index"; }
                        else
                        { ViewBag.RedirectUrl = host + "/marketing/Analysis/Index"; }
                    }
                    break;
                case "admin":
                    ViewBag.RedirectUrl = host + "/Admin/Instruction/Index";
                    break;
                default:
                    break;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult UnAuthorized()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode.Value == 404)
                {
                    // Handle 404 Not Found
                    // Return a specific view or message
                }
                else if (statusCode.Value == 500)
                {
                    // Handle 500 Internal Server Error
                    // Return a specific view or message
                }
                // Add more conditions for other status codes if needed
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}