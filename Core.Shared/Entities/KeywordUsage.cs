using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{   
     /// <summary>
     /// Keyword Usage:
     /// Presence of target keywords in the title tag, meta description, heading tags (H1, H2, etc.), 
     /// and throughout the content.
     /// Keyword density, ensuring a natural and appropriate distribution of keywords within the content.
     /// </summary>
    public class KeywordUsage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CrawledId { get; set; }
        public string? Keyword { get; set; }
        public int KeywordsInContent { get; set; }
        public int KeywordsInMetaDescription { get; set; }
        public int KeywordsInTitle { get; set; }
        public int KeywordsInHeading { get; set; }
        public bool IsMainKeyword { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Crawled? Crawled { get; set; }
    }
}
