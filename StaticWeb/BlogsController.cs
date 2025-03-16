using Microsoft.AspNetCore.Mvc;

namespace StaticWeb
{
    public class BlogsController : Controller
    {
        public IActionResult Page()
        {
            return View();
        }

        public IActionResult Post(string id)
        {
            var folder = Environment.CurrentDirectory + "\\wwwroot\\FrontEnd\\blogs\\";
            var path = folder + id + ".html";
            var isExist = System.IO.File.Exists(path);
            if (isExist)
            {
                var htmlContent = System.IO.File.ReadAllText(path).ToString();
                return View("Post", htmlContent);
            }
            return View("Post","No Post");
        }
    }
}
