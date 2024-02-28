using Microsoft.AspNetCore.Mvc;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class DefaultController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {

            return View();
        }
    }
}
