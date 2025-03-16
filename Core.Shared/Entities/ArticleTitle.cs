using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class ArticleTitle
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ArticleTypeId { get; set; }
        public string? Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsActive { get; set; }
        public virtual ArticleType? ArticleType { get; set; }
    }
}
