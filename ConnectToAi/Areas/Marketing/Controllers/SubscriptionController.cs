using Microsoft.AspNetCore.Mvc;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class SubscriptionController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
