using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    /// <summary>
    /// Page Loading Speed:
    /// Fast page loading times for a positive user experience.
    /// Optimization of images, code, and server response times to improve speed.
    /// </summary>
    public class PageLoadingSpeed
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CrawledId { get; set; }
        public string? URL { get; set; }
        public TimeSpan? PageLoadTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Crawled? Crawled { get; set; }
    }
}
