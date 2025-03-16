using ConnectToAi.Services;
using Core.Shared;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class ArticleController : BaseController
    {
        public ArticleController(ConfigService configService, ProjectService projectService, ArticleTypeService articleTypeService) : base(configService, projectService, articleTypeService)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.TotalProjectCreated = null;
            if (UserDetail != null && _projectService != null)
            {
                var projects = await _projectService.ListAsync(UserDetail.UserID);
                ViewBag.TotalProjectCreated = projects;
            }

            return View(new ArticleType());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleType articleType)
        {
            if (articleType != null && string.IsNullOrEmpty(articleType.ProjectId) && string.IsNullOrEmpty(articleType.MainKeywords))
            {
                await _articleTypeService.CreateAsync(articleType);
            }

            ViewBag.TotalProjectCreated = null;
            if (UserDetail != null && _projectService != null)
            {
                var projects = await _projectService.ListAsync(UserDetail.UserID);
                ViewBag.TotalProjectCreated = projects;
            }

            return View(articleType);
        }


        [HttpGet]
        public async Task<IActionResult> GetArticleType()
        {
            if (UserDetail != null && _articleTypeService != null && _projectService != null)
            {
                ViewBag.TotalProjectCreated = null;
                var projects = await _projectService.ListAsync(UserDetail.UserID);
                if (projects != null)
                {
                    ViewBag.TotalProjectCreated = projects;
                }

                //var articleTypes = await _articleTypeService.ListAsync(UserDetail.UserID);
                //return View(articleTypes);
            }
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetArticleTypeList(string projectId)
        {
            if (_articleTypeService != null)
            {
                var articleTypes = await _articleTypeService.ListAsync(projectId);

                return Json(articleTypes);
            }

            return Json("");
        }

        [HttpGet]
        public async Task<JsonResult> GerProject(string projectId)
        {
            
            if (UserDetail != null && _projectService != null)
            {
                var project = await _projectService.GetById(projectId);
                return Json(project);
            }

            return Json("");
        }
    }
}
