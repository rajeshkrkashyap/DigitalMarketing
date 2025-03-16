using Microsoft.AspNetCore.Mvc;

namespace StaticWeb
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
