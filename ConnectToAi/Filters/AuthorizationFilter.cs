using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Core.Shared;
using Newtonsoft.Json;

namespace ConnectToAi.Filters
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public AuthorizationFilter(string role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.Cookies.TryGetValue("ConnectToAi_DigitalMarketing_AuthToken", out string cookieValue);
            if (cookieValue == null)
            {
                //context.Result = new ForbidResult();
                context.HttpContext.Response.Redirect("/Identity/Account/LoginApp");
                return;
            }
            else
            {
                UserDetail userDetail = JsonConvert.DeserializeObject<UserDetail>(cookieValue);
                if (userDetail == null || userDetail.Role.ToLower() != _role.ToLower())
                {
                    //context.Result = new ForbidResult();
                    context.HttpContext.Response.Redirect("/Identity/Account/LoginApp");
                    return;
                }
            }
        }
    }
}
