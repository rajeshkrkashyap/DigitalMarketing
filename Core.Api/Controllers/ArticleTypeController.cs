using Core.Api.Models;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ArticleTypeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("List")]
        public List<ArticleType> LoadAllArticleTypes(string projectId)
        {
            return _dbContext.ArticleTypes.Where(t => t.ProjectId == projectId && t.IsActive).ToList();
        }

        [HttpPost("Count")]
        public int ArticleTypeCount(string projectId)
        {
            try
            {
                var count = _dbContext.ArticleTypes.Where(t => t.ProjectId == projectId && t.IsActive).Count();
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpPost("GetById")]
        public ArticleType GetArticleType(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var ArticleType = _dbContext.ArticleTypes.FirstOrDefault(inst => inst.Id == id && inst.IsActive == true);
                if (ArticleType != null)
                {
                    return ArticleType;
                }
            }
            return new ArticleType();
        }

        [HttpPost("Create")]
        public ArticleType Create(ArticleType ArticleType)
        {
            if (ArticleType != null)
            {
                var ArticleTypedb = _dbContext.ArticleTypes.Add(new ArticleType
                {
                    Id = ArticleType.Id,
                    ProjectId = ArticleType.ProjectId,
                    Name = ArticleType.Name,
                    Intent = ArticleType.Intent,
                    Description = ArticleType.Description,
                    UserProvidedKeywords = ArticleType.UserProvidedKeywords,
                    MainKeywords = ArticleType.MainKeywords,
                    OtherKeywords = ArticleType.OtherKeywords,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    IsActive = true,
                });
                _dbContext.SaveChanges();
                return ArticleTypedb.Entity;
            }
            return null;
        }

        [HttpPost("Update")]
        public bool Update(ArticleType ArticleType)
        {
            if (ArticleType != null)
            {
                var dbArticleType = _dbContext.ArticleTypes.FirstOrDefault(inst => inst.Id == ArticleType.Id);
                if (dbArticleType != null)
                {
                    dbArticleType.Name = ArticleType.Name;
                    dbArticleType.Intent = ArticleType.Intent;
                    dbArticleType.Description = ArticleType.Description;
                    dbArticleType.UserProvidedKeywords = ArticleType.UserProvidedKeywords;
                    dbArticleType.MainKeywords = ArticleType.MainKeywords;
                    dbArticleType.OtherKeywords = ArticleType.OtherKeywords;
                    dbArticleType.Updated = DateTime.UtcNow;
                    dbArticleType.IsActive = true;

                    _dbContext.ArticleTypes.Update(dbArticleType);
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
                var ArticleType = _dbContext.ArticleTypes.FirstOrDefault(inst => inst.Id == id);
                ArticleType.IsActive = false;
                _dbContext.ArticleTypes.Update(ArticleType);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
