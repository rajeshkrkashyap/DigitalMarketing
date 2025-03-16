using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    /// <summary>
    /// <meta name="twitter:image" content="https://images.astroyogi.com/astroyogi2017/english/images/panchang/swastik_tithi.png" />
    /// <meta property = "og:image" content="https://images.astroyogi.com/astroyogi2017/english/images/panchang/swastik_tithi.png" />
    /// <meta property = "og:title" content="Today Rahu kaal - RahuKalam Timings Calculator For City" />
    /// <meta property = "og:description" content="Today Rahu Kaal, the most inauspicious time, should be avoided for religious events. Find Rahukalam Timings for your city today" />
    /// <meta property = "og:keywords" content="rahu kaal today, rahu kaal, rahu kalam, rahu kalam calculator, rahu kaal time,rahukalam timings today" />
    /// <meta name = "theme-color" content="#FFCC00" />
    /// <meta name = "copyright" content="Astroyogi.com" />
    /// <meta property = "fb:app_id" content="410278222341602" />
    /// <meta property = "fb:pages" content="168694379812269" />
    /// <meta name = "twitter:card" content="summary_large_image" />
    /// <meta name = "twitter:title" content="Today Rahu kaal - RahuKalam Timings Calculator For City" />
    /// <meta name = "twitter:site" content="@Astroyogi" />
    /// <meta name = "twitter:description" content="Today Rahu Kaal, the most inauspicious time, should be avoided for religious events. Find Rahukalam Timings for your city today" />
    /// <meta name = "twitter:creator" content="@Astroyogi" />
    /// <meta name = "description" content="Today Rahu Kaal, the most inauspicious time, should be avoided for religious events. Find Rahukalam Timings for your city today" />
    /// <meta name = "keywords" content="rahu kaal today, rahu kaal, rahu kalam, rahu kalam calculator, rahu kaal time,rahukalam timings today" />
    /// </summary>
    public class Blog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ProjectId { get; set; }
        public string? Title { get; set; }
        public string? SmallDescription { get; set; }
        public string? MediaType { get; set; }
        public string? MediaURL { get; set; }
        public string? PageURL { get; set; }
        public string? Auther { get; set; }
        public string? MetaName { get; set; }  // this will store data in JSON ARRAY
        public string? MetaProperty { get; set; } // this will store data in JSON ARRAY
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Project? Project { get; set; }
    }
}
