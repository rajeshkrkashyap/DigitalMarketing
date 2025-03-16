using Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class ManageController : BaseController
    {
        public ManageController(ConfigService configService) : base(configService)
        {

        }
        public async Task<IActionResult> WebSite()
        {

            return View();
        }
    }
}
