using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    /// <summary>
    /// Internal Linking:
    /// Effective use of internal links to connect related pages within the website. Anchor text that 
    /// provides context for the linked content.
    /// </summary>
    public class InternalLinking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CrawledId { get; set; }
        public string? LinkText { get; set; }
        public string? Link { get; set; }
        public bool IsInternal { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Crawled? Crawled { get; set; }
    }
}
