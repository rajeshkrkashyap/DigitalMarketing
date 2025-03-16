using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class AccuracyAndCredibility
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ContentQualityId { get; set; }
        public int WhoIsDoaminScore { get; set; }
        public bool HasAuthoritativeSources { get; set; }
        public bool AuthoredByExperts { get; set; }
        public bool HasCitations { get; set; }
        public bool HasCitationsInParentheses { get; set; }
        public string? ReferencesSectionContent { get; set; }
        public double ReputationScore { get; set; }
        public int CitationCount { get; set; }
        public string? AccuracyCredibilityScore { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }

        public virtual ContentQuality? ContentQuality { get; set; }
    }
}
