using Core.Shared.Entities;
using Core.Service.Server;
using HtmlAgilityPack;
using Serilog;
using System.Reflection.Metadata;

namespace OnPageSsoScoreServer
{
    public class OnPageSeoScore
    {
        private readonly ProjectService _projectService;
        private readonly LoggerConfiguration _loggerConfiguration;
        private readonly HtmlDocument _document;
        public Crawled? Crawled { get; set; }
        public OnPageSeoScore(HtmlDocument htmlDocument, ProjectService projectService, LoggerConfiguration loggerConfiguration)
        {
            _document = htmlDocument;
            _projectService = projectService;
            _loggerConfiguration = loggerConfiguration;
        }       
    }
}