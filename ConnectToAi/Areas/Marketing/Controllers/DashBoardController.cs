using Microsoft.AspNetCore.Mvc;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class DashBoardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
