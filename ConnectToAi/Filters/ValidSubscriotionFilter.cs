using ConnectToAi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Core.Shared;
using Newtonsoft.Json;
using NuGet.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace ConnectToAi.Filters
{
    public class ValidSubscriotionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Cookies.TryGetValue("ConnectToAi_DigitalMarketing_AuthToken", out string cookieValue);
            context.HttpContext.Request.Cookies.TryGetValue("cookie_AppUserSetting", out string appSettingCookieValue);
            var host = context.HttpContext.Request.Scheme + "://" + context.HttpContext.Request.Host.Value;

            if (cookieValue == null || appSettingCookieValue == null)
            {
                var redirectUrl = host + "/Identity/Account/Login";
                context.HttpContext.Response.Redirect(redirectUrl);
                return;
            }

            AppSettingCookie appSettingCookie = JsonConvert.DeserializeObject<AppSettingCookie>(appSettingCookieValue);

            if (appSettingCookie.SubscriptionEndDate.Value.Date < DateTime.UtcNow.Date)
            {
                var redirectUrl = host + "/Student/AppUserSettings/SubcriptionEnd";
                context.HttpContext.Response.Redirect(redirectUrl);
            }
        }
    }
}
