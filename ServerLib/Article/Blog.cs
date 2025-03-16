using Core.Service.Server;
using Core.Shared.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.Article
{
    public class BlogModel : BaseModel
    {
        public BlogModel(HtmlDocument document, CrawledService crawledService) : base(document, crawledService)
        {

        }
    }
}
