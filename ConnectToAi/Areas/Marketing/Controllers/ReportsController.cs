using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Api.Models;
using Core.Shared.Entities;
using ConnectToAi.Services;
using Core.Shared;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    [Area("Marketing")]
    public class ReportsController : BaseController
    {
        public ReportsController(ConfigService configService) : base(configService)
        {
        }

        // GET: Marketing/Reports
        public async Task<IActionResult> Index(string projectId)
        {
            if (!string.IsNullOrEmpty(projectId))
            {
                using (CrawledService crawledService = new(_configService))
                {
                    var crawledList = await crawledService.ListAsync(projectId);

                    if (crawledList.Count() > 0)
                    {
                        return View(crawledList);
                    }
                }
            }
            return View(new List<Crawled>());
        }
    }
}
