using ConnectToAi.Areas.Marketing.Models;
using ConnectToAi.Services;
using Core.Shared;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class AnalysingController : BaseController
    {
        public AnalysingController(ConfigService configService) : base(configService)
        {

        }
        public IActionResult Index()
        {
            if (UserDetail!=null)
            {
                using (ProjectService projectService = new(_configService))
                {
                    var projects = projectService.ListAsync(UserDetail.UserID).Result;

                    if (projects.Count()>0)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }

            }
            return View(new ProjectViewModel());
        }
        public IActionResult AddProject(ProjectViewModel projectViewModel)
        {
            using (ProjectService projectService = new(_configService))
            {
                var addProject = new Project
                {
                    Name = projectViewModel.Name,
                    Description = projectViewModel.Description,
                    URL = projectViewModel.URL,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    IsActive = true,
                    AnalysisStatus = "Start",
                    AppUserId = UserDetail.UserID
                };

                var project = projectService.CreateAsync(addProject).Result;

                if (!string.IsNullOrEmpty(project.URL))
                {
                    SendProjectForAnalysis(project.Id);
                }
            }

            return RedirectToAction("Index", "Dashboard");
        }

    }
}
