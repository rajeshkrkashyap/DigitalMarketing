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
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalProjectCreated = null;
            if (UserDetail != null)
            {
                var projects = await _projectService.ListAsync(UserDetail.UserID);
                ViewBag.TotalProjectCreated = projects;
            }
            return View();
        }

        [HttpPost]
        public JsonResult StartAnalysis(string id)
        {
            if (UserDetail != null)
            {
                if (base.SendProjectForAnalysis(id) == true)
                {
                    return Json("success");
                }
            }
            return Json("failed");
        }

        [HttpPost]
        public async Task<JsonResult> GetProjectStatus(string id)
        {
            if (UserDetail != null)
            {
                var project = await _projectService.GetById(id);
                return Json(project);

            }
            return Json(0);
        }
    }
}
