using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared
{
    public class AppUserViewModel
    {
        public string? Id { get; set; }
        public long Tokens { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
    }
}
