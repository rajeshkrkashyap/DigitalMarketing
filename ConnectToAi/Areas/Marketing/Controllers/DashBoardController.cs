using ConnectToAi.Services;
using Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class DashBoardController : BaseController
    {
        public DashBoardController(ConfigService configService, ProjectService projectService) : base(configService, projectService)
        {

        }
        public IActionResult Index()
        {
            return View();
        } 
    }
}
