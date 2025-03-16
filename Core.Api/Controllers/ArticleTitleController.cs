using Core.Api.Models;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleTitleController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ArticleTitleController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("List")]
        public List<ArticleTitle> LoadAllArticleTitles(string articleTypeId)
        {
            return _dbContext.ArticleTitles.Where(t => t.ArticleTypeId == articleTypeId).ToList();
        }

        [HttpPost("Count")]
        public int ArticleTitleCount(string articleTypeId)
        {
            try
            {
                var count = _dbContext.ArticleTitles.Where(t => t.ArticleTypeId == articleTypeId && t.IsActive == true).Count();
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        [HttpPost("GetById")]
        public ArticleTitle GetArticleTitle(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var ArticleTitle = _dbContext.ArticleTitles.FirstOrDefault(inst => inst.Id == id && inst.IsActive == true);
                if (ArticleTitle != null)
                {
                    return ArticleTitle;
                }
            }
            return new ArticleTitle();
        }

        [HttpPost("Create")]
        public ArticleTitle Create(ArticleTitle ArticleTitle)
        {
            if (ArticleTitle != null)
            {
                var ArticleTitledb = _dbContext.ArticleTitles.Add(new ArticleTitle
                {
                    Id = ArticleTitle.Id,
                    ArticleTypeId = ArticleTitle.ArticleTypeId,
                    Title = ArticleTitle.Title,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    IsActive = true,
                });
                _dbContext.SaveChanges();
                return ArticleTitledb.Entity;
            }
            return null;
        }

        [HttpPost("Update")]
        public bool Update(ArticleTitle ArticleTitle)
        {
            if (ArticleTitle != null)
            {
                var dbArticleTitle = _dbContext.ArticleTitles.FirstOrDefault(inst => inst.Id == ArticleTitle.Id);
                if (dbArticleTitle != null)
                {
                    dbArticleTitle.Title = ArticleTitle.Title;
                    dbArticleTitle.Updated = DateTime.UtcNow;
                    dbArticleTitle.IsActive = true;

                    _dbContext.ArticleTitles.Update(dbArticleTitle);
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
                var articleTitle = _dbContext.ArticleTitles.FirstOrDefault(inst => inst.Id == id);
                if (articleTitle != null)
                {
                    articleTitle.IsActive = false;
                    _dbContext.ArticleTitles.Update(articleTitle);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
