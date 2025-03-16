using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class ArticleType
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Intent { get; set; } 
        public string? Description { get; set; }
        public string? UserProvidedKeywords { get; set; }
        public string? MainKeywords { get; set; }
        public string? OtherKeywords { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }
        public virtual Project? Project { get; set; }
    }
}
