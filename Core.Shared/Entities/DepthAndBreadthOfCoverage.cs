using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class DepthAndBreadthOfCoverage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ContentQualityId { get; set; }
        public string KeyAspects { get; set; }
        public string ContentAnalysis { get; set; }
        public string DepthOfCoverage { get; set; }
        public string BreadthOfCoverage { get; set; }
        public string SupportingElements { get; set; }
        public string Feedback { get; set; }
        public int TotalScore { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }
        public virtual ContentQuality? ContentQuality { get; set; }
    }
}
