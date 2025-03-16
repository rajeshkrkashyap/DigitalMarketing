using Core.Api.Models;
using Core.Shared;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Twilio.TwiML.Voice;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;
        public BlogController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("List")]
        public List<Blog> LoadAllBlogs()
        {
            return _dbContext.Blogs.Where(t => t.IsActive == true).ToList();
        }

        [HttpPost("GetById")]
        public Blog GetBlog(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var blog = _dbContext.Blogs.FirstOrDefault(inst => inst.Id == id && inst.IsActive == true);
                if (blog != null)
                {
                    return blog;
                }
            }
            return null;
        }

        [HttpPost("IsExist")]
        public bool GetIsExist(string title)
        {
            var blog = _dbContext.Blogs.FirstOrDefault(inst => inst.Title == title && inst.IsActive == true);

            if (blog == null)
            {
                return false;
            }
            return true;
        }

        [HttpPost("Create")]
        public Blog Create(Blog blog)
        {
            if (blog != null)
            {
                var blogdb = _dbContext.Blogs.Add(new Blog
                {
                    Id = blog.Id,
                    MediaType = blog.MediaType,
                    Title = blog.Title,
                    MediaURL = blog.MediaURL,
                    MetaName = blog.MetaName,
                    MetaProperty = blog.MetaProperty,
                    SmallDescription = blog.SmallDescription,
                    PageURL = blog.PageURL,
                    Auther = blog.Auther,

                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    IsActive = true,
                });
                _dbContext.SaveChanges();
                return blogdb.Entity;
            }
            return null;
        }

        [HttpPost("Update")]
        public bool Update(Blog blog)
        {
            if (blog != null)
            {
                var dbBlog = _dbContext.Blogs.FirstOrDefault(inst => inst.Id == blog.Id);
                if (dbBlog != null)
                {
                    dbBlog.MediaType = blog.MediaType;
                    dbBlog.Title = blog.Title;
                    dbBlog.MediaURL = blog.MediaURL;
                    dbBlog.MetaName = blog.MetaName;
                    dbBlog.MetaProperty = blog.MetaProperty;
                    dbBlog.SmallDescription = blog.SmallDescription;
                    dbBlog.PageURL = blog.PageURL;
                    dbBlog.Auther = blog.Auther;
                    dbBlog.Updated = DateTime.UtcNow;

                    _dbContext.Blogs.Update(dbBlog);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }


        [HttpPost("Delete")]
        public bool Delete(string id)
        {
            if (id != null)
            {
                var blog = _dbContext.Blogs.FirstOrDefault(inst => inst.Id == id);
                if (blog != null)
                {
                    blog.IsActive = false;
                    _dbContext.Blogs.Update(blog);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
