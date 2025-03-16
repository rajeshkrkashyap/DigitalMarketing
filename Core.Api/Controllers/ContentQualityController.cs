using Core.Api.Models;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{
    public class ContentQualityController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ContentQualityController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("Create")]
        public bool Create(ContentQuality contentQuality)
        {
            if (contentQuality != null)
            {
                try
                {
                    _dbContext.ContentQualities.Add(new ContentQuality
                    {
                        Id = contentQuality.Id,
                        PageTitle = contentQuality.PageTitle,
                        UniquenessAndOriginality = contentQuality.UniquenessAndOriginality,
                        EngagementAndInteractivity = contentQuality.EngagementAndInteractivity,
                        UserExperience = contentQuality.UserExperience,
                        IsRelevanceToTheTopic = contentQuality.IsRelevanceToTheTopic,
                        IsAccuracyAndCredibility = contentQuality.IsAccuracyAndCredibility,
                        IsClarityAndReadability = contentQuality.IsClarityAndReadability,
                        IsDepthAndBreadthOfCoverage = contentQuality.IsDepthAndBreadthOfCoverage,
                        IsReputationAndAuthority = contentQuality.IsReputationAndAuthority,
                        IsPurposeAndIntent = contentQuality.IsPurposeAndIntent,
                        IsFeedbackAndMetrics = contentQuality.IsFeedbackAndMetrics,
                        IsActive = true,
                    });
                    _dbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
            return false;
        }
    }
}
