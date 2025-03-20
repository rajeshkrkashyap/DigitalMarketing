using Core.Api.Models;
using Core.Shared;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Twilio.TwiML.Voice;
using Core.Shared.Extentions;
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
        public async Task<MainResponse> Blogs()
        {
            try
            {
                var blogList = await _dbContext.Blogs.Where(b => b.IsActive)
                                                     .Select(s => new Blog
                                                     {
                                                         Id = s.Id,
                                                         Title = s.Title,
                                                         Updated = s.Updated,
                                                         FileName = s.FileName,
                                                         IsActive = s.IsActive,
                                                     }).OrderBy(o => o.Updated).ToListAsync();

                return new MainResponse
                {
                    Content = blogList,
                    IsSuccess = true,
                    ErrorMessage = ""
                };
            }
            catch (Exception)
            {
            }
            return new MainResponse
            {
                Content = new List<Blog>(),
                IsSuccess = true,
                ErrorMessage = ""
            };
        }

        [HttpPost("GetById")]
        public MainResponse GetBlog(string id)
        {
            var response = new MainResponse
            {
                Content = _dbContext.Blogs.FirstOrDefault(inst => inst.Id == id),
                IsSuccess = true,
                ErrorMessage = ""
            };
            return response;
        }

        [HttpPost("Create")]
        public async Task<bool> Create(Blog blog)
        {
            if (blog != null)
            {
                try
                {
                    var count = await _dbContext.Blogs.CountAsync(id => id.Id == blog.Id);
                    if (count == 0)
                    {
                        if (!string.IsNullOrWhiteSpace(blog.Title)) blog.Title = blog.Title.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.FileName)) blog.FileName = blog.FileName.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.CtaAction)) blog.CtaAction = blog.CtaAction.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.ContentType)) blog.ContentType = blog.ContentType.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.TargetAudience)) blog.TargetAudience = blog.TargetAudience.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.AdditionalNotes)) blog.AdditionalNotes = blog.AdditionalNotes.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.CompetitorUrls)) blog.CompetitorUrls = blog.CompetitorUrls.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.ExternalLinkTopic)) blog.ExternalLinkTopic = blog.ExternalLinkTopic.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.InternalLinkTopic)) blog.InternalLinkTopic = blog.InternalLinkTopic.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.PrimaryKeyword)) blog.PrimaryKeyword = blog.PrimaryKeyword.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.SecondaryKeywords)) blog.SecondaryKeywords = blog.SecondaryKeywords.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.TargetLocation)) blog.TargetLocation = blog.TargetLocation.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.ToneOfVoice)) blog.ToneOfVoice = blog.ToneOfVoice.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.WordCountMax)) blog.WordCountMax = blog.WordCountMax.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.WordCountMin)) blog.WordCountMin = blog.WordCountMin.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.SearchIntent)) blog.SearchIntent = blog.SearchIntent.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.Content)) blog.Content = blog.Content.TrimTags("<p>", "</p>");
                        if (!string.IsNullOrWhiteSpace(blog.MetaTags)) blog.MetaTags = blog.MetaTags.TrimTags("<p>", "</p>");

                        blog.IsActive = true;
                        blog.Updated = DateTime.UtcNow;

                        await _dbContext.Blogs.AddAsync(blog);
                        await _dbContext.SaveChangesAsync();
                        return true;
                    }
                }
                catch (Exception)
                {
                }
            }
            return false;
        }

        [HttpPost("Update")]
        public async Task<bool> Update(Blog blog)
        {
            if (blog != null)
            {
                try
                {
                    var dbBlog = await _dbContext.Blogs.FirstOrDefaultAsync(id => id.Id == blog.Id);
                    if (dbBlog == null)
                    {
                        return false;
                    }

                    if (!string.IsNullOrWhiteSpace(blog.Title)) blog.Title = blog.Title.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.FileName)) blog.FileName = blog.FileName.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.CtaAction)) blog.CtaAction = blog.CtaAction.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.ContentType)) blog.ContentType = blog.ContentType.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.TargetAudience)) blog.TargetAudience = blog.TargetAudience.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.AdditionalNotes)) blog.AdditionalNotes = blog.AdditionalNotes.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.CompetitorUrls)) blog.CompetitorUrls = blog.CompetitorUrls.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.ExternalLinkTopic)) blog.ExternalLinkTopic = blog.ExternalLinkTopic.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.InternalLinkTopic)) blog.InternalLinkTopic = blog.InternalLinkTopic.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.PrimaryKeyword)) blog.PrimaryKeyword = blog.PrimaryKeyword.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.SecondaryKeywords)) blog.SecondaryKeywords = blog.SecondaryKeywords.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.TargetLocation)) blog.TargetLocation = blog.TargetLocation.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.ToneOfVoice)) blog.ToneOfVoice = blog.ToneOfVoice.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.WordCountMax)) blog.WordCountMax = blog.WordCountMax.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.WordCountMin)) blog.WordCountMin = blog.WordCountMin.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.SearchIntent)) blog.SearchIntent = blog.SearchIntent.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.Content)) blog.Content = blog.Content.TrimTags("<p>", "</p>");
                    if (!string.IsNullOrWhiteSpace(blog.MetaTags)) blog.MetaTags = blog.MetaTags.TrimTags("<p>", "</p>");

                    dbBlog.IsActive = true;
                    dbBlog.Updated = DateTime.UtcNow;

                    _dbContext.Entry(dbBlog).Property(u => u.Id).IsModified = false;

                    _dbContext.Blogs.Update(dbBlog);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                }
            }
            return false;
        }
    }
}
