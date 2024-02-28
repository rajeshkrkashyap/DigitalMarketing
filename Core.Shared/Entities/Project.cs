using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class Project
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? AppUserId { get; set; }
        public string? Name { get; set; }
        public string? URL { get; set; }
        public string? Description { get; set; }
        public string AnalysisStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual AppUser? AppUser { get; set; }
    }
}
