using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class TechnicalSEO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CrawledId { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Crawled? Crawled { get; set; }
    }
}
