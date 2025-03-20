using Azure.Storage.Blobs;
using ConnectToAi.Services;
using Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ConnectToAi.Controllers
{
    public class BlogsController : BaseController
    {
        public BlogsController(ConfigService configService, BlogService blogService) : base(configService, blogService)
        {

        }
        public async Task<IActionResult> Page()
        {
            if (_blogService != null)
            {
                return View(await _blogService.BlogList() );
            }
            return View();
        }

        public async Task<IActionResult> Post(string id)
        {
            var folder = Environment.CurrentDirectory + "\\wwwroot\\blogs\\";
            var path = folder + id + ".html";
            var isExist = System.IO.File.Exists(path);
            if (isExist)
            {
                var htmlContent = System.IO.File.ReadAllText(path).ToString();
                if (_blogService != null)
                {
                    ViewBag.HtmlContent = htmlContent;
                    return View("Post", await _blogService.BlogList());
                }
            }

            return View("Post", "No Post");
        }
    }
}
