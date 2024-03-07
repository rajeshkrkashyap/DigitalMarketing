using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Api.Models;
using Core.Shared.Entities;
using System;
using System.Linq;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CrawledController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public CrawledController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("Create")]
        public bool Create(Crawled crawled)
        {
            if (crawled != null)
            {
                if (Update(crawled))
                {
                    return true;
                }

                _dbContext.Crawleds.Add(new Crawled
                {
                    Id = crawled.Id,
                    URL = crawled.URL,
                    PageContent = crawled.PageContent,
                    ProjectId = crawled.ProjectId,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    IsActive = true,
                });
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        [HttpPost("Update")]
        public bool Update(Crawled Crawled)
        {
            if (Crawled != null)
            {
                var dbCrawled = _dbContext.Crawleds.FirstOrDefault(c => c.URL == Crawled.URL &&
                                                                       c.ProjectId == Crawled.ProjectId);
                if (dbCrawled != null)
                {
                    dbCrawled.PageContent = Crawled.PageContent;
                    dbCrawled.Updated = DateTime.UtcNow;
                    _dbContext.Crawleds.Update(dbCrawled);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        [HttpPost("UpdateAnalysisStatus")]
        public bool UpdateAnalysisStatus(Crawled Crawled)
        {
            if (Crawled != null)
            {
                var dbCrawled = _dbContext.Crawleds.FirstOrDefault(c => c.Id ==Crawled.Id);
                if (dbCrawled != null)
                {
                    dbCrawled.AnalysisStatus = Crawled.AnalysisStatus;
                    dbCrawled.Updated = DateTime.UtcNow;
                    _dbContext.Crawleds.Update(dbCrawled);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
