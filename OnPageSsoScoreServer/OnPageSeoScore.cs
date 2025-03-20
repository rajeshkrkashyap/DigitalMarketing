using Core.Shared.Entities;
using HtmlAgilityPack;
using Serilog;
using System.Reflection.Metadata;
using Core.Service.ServerCommon;

namespace OnPageSeoScoreServer
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