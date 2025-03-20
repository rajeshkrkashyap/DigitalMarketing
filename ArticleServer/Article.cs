using Core.Service.ServerCommon;
using Core.Shared.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleServer
{
    public class Article
    {
        
        private readonly LoggerConfiguration _loggerConfiguration;

        public string ProjectId { get; set; }
        public string CrawledId { get; set; }
        public Article(LoggerConfiguration loggerConfiguration)
        {
            _loggerConfiguration = loggerConfiguration;
        }

        public string GenrateTitle()
        {
            return null;

        } 

    }
}
