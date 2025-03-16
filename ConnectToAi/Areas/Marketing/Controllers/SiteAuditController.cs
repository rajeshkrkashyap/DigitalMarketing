using ConnectToAi.Services;
using Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class SiteAuditController : BaseController
    {
        public SiteAuditController(ConfigService configService, ProjectService projectService) : base(configService, projectService)
        {

        }
        public async Task<IActionResult> Analysis()
        {
            ViewBag.TotalProjectCreated = null;
            if (UserDetail != null && _projectService != null)
            {
                var projects = await _projectService.ListAsync(UserDetail.UserID);
                ViewBag.TotalProjectCreated = projects;
            }
            return View();
        }

        public async Task<IActionResult> SpeedTest()
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
            if (UserDetail != null && !string.IsNullOrEmpty(id))
            {
                var project = await _projectService.GetById(id);
                return Json(project);

            }
            return Json(0);
        }
    }
}
