using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    /// <summary>
    /// Relevance to the Topic: Determine whether the content aligns with the main topic or purpose
    /// of the webpage. Irrelevant or off-topic content can detract from the overall quality. 
    /// </summary>
    public class RelevanceToTheTopic
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ContentQualityId { get; set; }
        public string? UnderstandingOfTopic { get; set; }
        public string? AnalysisOfContent { get; set; }
        public string? ConsistencyCheck { get; set; }
        public string? RelevanceAssessment { get; set; }
        public string? ConsiderationOfAudienceIntent { get; set; }
        public string? FeedbackOfContent { get; set; }
        public int? TotalScore { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }

        public virtual ContentQuality? ContentQuality { get; set; }
    }
}
