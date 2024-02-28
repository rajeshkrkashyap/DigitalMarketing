using Microsoft.AspNetCore.Mvc;

namespace ConnectToAi.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return LocalRedirect("/Identity/Account/LoginApp");
            //return View();
        }
    }
}
