using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class Crawled
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ProjectId { get; set; }
        public string? URL { get; set; }
        public string? PageContent { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Project? Project { get; set; }
    }
}
