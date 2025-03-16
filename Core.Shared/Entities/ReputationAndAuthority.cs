using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class ReputationAndAuthority
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ContentQualityId { get; set; }
        public string? DomainAgeInYears { get; set; }
        public string? BacklinkCount { get; set; }
        public string? SocialEngagement { get; set; }
        public string? AuthorExpertise { get; set; }
        public double ReputationScore { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }
        public virtual ContentQuality? ContentQuality { get; set; }
    }
}
