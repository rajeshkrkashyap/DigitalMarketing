using ConnectToAi.Services;
using Core.Service.Azure;
using Core.Shared;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Core.Shared.Extentions;
namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class CompetitorArticleController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly BlogService _blogService;
        private readonly CompetitorArticleService _competitorArticleService;
        public CompetitorArticleController(ConfigService configService, ProjectService projectService, BlogService blogService, 
                                           IWebHostEnvironment env, CompetitorArticleService competitorArticleService) : base(configService, projectService, blogService)
        {
            _env = env;
            _blogService = blogService;
            _competitorArticleService = competitorArticleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.BlogList(); // Await the async method
            return View(blogs.ToList()); // Convert IEnumerable to List, if necessary
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogs = await _blogService.BlogList(); //Await the async method
            return View(blogs.ToList()); //Convert IEnumerable to List, if necessary
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Blog());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blog model)
        {
            var fileName = model.Title.Trim().Replace(" ", "-");
            model.FileName = fileName;
            model.MetaTags = model.MetaTags.TrimTags("<p>", "</p>");
            await _blogService.Create(model);

            SaveIntoHTMLFile(model.Content, fileName);

            return View(model);
        }

        [HttpGet]

        public async Task<IActionResult> Edit(string id)
        {
            var blog = await _blogService.BlogGetById(id);
            return View(blog);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Blog blog)
        {
            if (ModelState.IsValid)
            {
                var fileName = blog.Title.Trim().Replace(" ", "-");
                blog.FileName = fileName;
                var status = await _blogService.Update(blog);
                SaveIntoHTMLFile(blog.Content, fileName);
            }

            return View(blog);
        }

        [HttpPost]
        public async Task<JsonResult> GeneratePrimaryKeywords([FromBody] Blog request)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(request.Title))
            {
                result= await _competitorArticleService.AiGeneratePrimaryKeywords(request);
            }

            return Json(new { primaryKeywords = result });
        }

        [HttpPost]
        public async Task<JsonResult> GenerateSecondaryKeywords([FromBody] Blog request)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(request.Title))
            {
                result= await _competitorArticleService.AiGenerateSecondaryKeywords(request);
            }

            return Json(new { secondaryKeywords = result });
        }


        [HttpPost]
        public async Task<JsonResult> GenerateContent([FromBody] Blog request)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(request.Title))
            {
                result= await _competitorArticleService.AiGenerateContent(request);
            }

            return Json(new { content = result });
        }

        [HttpPost]
        public async Task<JsonResult> GenerateMetaTags([FromBody] Blog request)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(request.Title))
            {
                result = await _competitorArticleService.AiGenerateMetaTags(request);
            }

            return Json(new { metaTags = result });
        }


        private void SaveIntoHTMLFile(string content, string fileName)
        {
            string path = Path.Combine(_env.WebRootPath, "blogs\\", fileName + ".html");
            System.IO.File.WriteAllText(path, content);
        }
    }
}
