using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class ClarityAndReadability
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ContentQualityId { get; set; }
        public string? LanguageClarity { get; set; }
        public string? ExplanationOfComplexConcepts { get; set; }
        public string? GrammarAndSpelling { get; set; }
        public string? Formatting { get; set; }
        public int TotalScore { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }

        public virtual ContentQuality? ContentQuality { get; set; }
    }
}
