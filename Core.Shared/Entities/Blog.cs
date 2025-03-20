using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class Blog
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? FileName { get; set; }
        public string? Title { get; set; }
        public string? MetaTags { get; set; }
        public string? ContentType { get; set; }
        public string? TargetAudience { get; set; }
        public string? PrimaryKeyword { get; set; }
        public string? SecondaryKeywords { get; set; }
        public string? CtaAction { get; set; }
        public string? TargetLocation { get; set; }
        public string? ToneOfVoice { get; set; }
        public string? WordCountMin { get; set; }
        public string? WordCountMax { get; set; }
        public string? AdditionalNotes { get; set; }
        public string? InternalLinkTopic { get; set; }
        public string? ExternalLinkTopic { get; set; }
        public string? CompetitorUrls { get; set; }
        public string? Content { get; set; }
        public string? SearchIntent { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }
        public Project? Project { get; set; }
    }
}
