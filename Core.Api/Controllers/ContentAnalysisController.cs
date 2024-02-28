using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Shared.Entities;
using System.Collections.Generic;
using System;
using Core.Api.Models;
using System.Linq;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentAnalysisController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ContentAnalysisController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPost("Create")]
        public bool Create(ContentAnalysis contentAnalysis)
        {
            if (contentAnalysis != null)
            {
                try
                {

                    _dbContext.ContentAnalysiss.Add(new ContentAnalysis
                    {
                        Id = contentAnalysis.Id,
                        CrawledId = contentAnalysis.CrawledId,
                        MetaTagKeywords = contentAnalysis.MetaTagKeywords,
                        Headings = contentAnalysis.Headings,
                        KeywordFrequency = contentAnalysis.KeywordFrequency,
                        MetaDescription = contentAnalysis.MetaDescription,
                        Title = contentAnalysis.Title,
                        ImageDetail = contentAnalysis.ImageDetail,
                    });
                    _dbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {

                }

            }
            return false;
        }

        [HttpPost("UpdateMetaTagKeywords")]
        public bool UpdateMetaTagKeywords(ContentAnalysis contentAnalysis)
        {
            if (contentAnalysis != null)
            {
                var dbContentAnalysis = _dbContext.ContentAnalysiss.FirstOrDefault(c => c.CrawledId == contentAnalysis.CrawledId);
                if (dbContentAnalysis != null)
                {
                    dbContentAnalysis.MetaTagKeywords = contentAnalysis.MetaTagKeywords;

                    _dbContext.ContentAnalysiss.Update(dbContentAnalysis);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        [HttpPost("UpdateHeadings")]
        public bool UpdateHeadings(ContentAnalysis contentAnalysis)
        {
            if (contentAnalysis != null)
            {
                var dbContentAnalysis = _dbContext.ContentAnalysiss.FirstOrDefault(c => c.CrawledId == contentAnalysis.CrawledId);
                if (dbContentAnalysis != null)
                {
                    dbContentAnalysis.Headings = contentAnalysis.Headings;

                    _dbContext.ContentAnalysiss.Update(dbContentAnalysis);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        [HttpPost("UpdateKeywordFrequency")]
        public bool UpdateKeywordFrequency(ContentAnalysis contentAnalysis)
        {
            if (contentAnalysis != null)
            {
                var dbContentAnalysis = _dbContext.ContentAnalysiss.FirstOrDefault(c => c.CrawledId == contentAnalysis.CrawledId);
                if (dbContentAnalysis != null)
                {
                    dbContentAnalysis.KeywordFrequency = contentAnalysis.KeywordFrequency;

                    _dbContext.ContentAnalysiss.Update(dbContentAnalysis);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        [HttpPost("UpdateMetaDescription")]
        public bool UpdateMetaDescription(ContentAnalysis contentAnalysis)
        {
            if (contentAnalysis != null)
            {
                var dbContentAnalysis = _dbContext.ContentAnalysiss.FirstOrDefault(c => c.CrawledId == contentAnalysis.CrawledId);
                if (dbContentAnalysis != null)
                {
                    dbContentAnalysis.MetaDescription = contentAnalysis.MetaDescription;

                    _dbContext.ContentAnalysiss.Update(dbContentAnalysis);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        [HttpPost("UpdateTitle")]
        public bool UpdateTitle(ContentAnalysis contentAnalysis)
        {
            if (contentAnalysis != null)
            {
                var dbContentAnalysis = _dbContext.ContentAnalysiss.FirstOrDefault(c => c.CrawledId == contentAnalysis.CrawledId);
                if (dbContentAnalysis != null)
                {
                    dbContentAnalysis.Title = contentAnalysis.Title;

                    _dbContext.ContentAnalysiss.Update(dbContentAnalysis);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

    }
}
