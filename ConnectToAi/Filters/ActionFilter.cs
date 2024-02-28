using ConnectToAi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Core.Shared;
using Newtonsoft.Json;
using NuGet.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace ConnectToAi.Filters
{
    public class ActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Cookies.TryGetValue("ConnectToAi_DigitalMarketing_AuthToken", out string cookieValue);
            var host = context.HttpContext.Request.Scheme + "://" + context.HttpContext.Request.Host.Value;

            if (cookieValue == null)
            {
                var redirectUrl = host + "/Identity/Account/Login";
                context.HttpContext.Response.Redirect(redirectUrl);
                return;
            }

            UserDetail userDetail = JsonConvert.DeserializeObject<UserDetail>(cookieValue);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(userDetail.AccessToken) as JwtSecurityToken;
            if (jwtToken != null)
            {
                if (jwtToken != null && jwtToken.ValidTo < DateTime.UtcNow)
                {
                    var redirectUrl = host + "/Identity/Account/Login";
                    context.HttpContext.Response.Redirect(redirectUrl);
                    return;
                }
            }
            if (userDetail != null && userDetail.Role.ToLower() != context.RouteData.Values["area"].ToString().ToLower())
            {
                var redirectUrl = host + "/Home/UnAuthorized";
                context.HttpContext.Response.Redirect(redirectUrl);
                return;

            }

            context.HttpContext.Request.Cookies.TryGetValue("ReturnURL", out string? returnURLcookieValue);
            if (returnURLcookieValue != null)
            {
                var redirectUrl = host + "/Identity/Account/screenlock";
                context.HttpContext.Response.Redirect(redirectUrl);
            }
        }
    }
}
