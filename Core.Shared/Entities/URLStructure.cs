using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class URLStructure
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CrawledId { get; set; }
        public string? URL { get; set; }
        public bool IsCleanURL { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Crawled? Crawled { get; set; }
    }
}
