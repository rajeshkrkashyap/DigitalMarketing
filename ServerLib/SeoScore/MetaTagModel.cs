using Core.Service.Azure;
using Core.Service.SeoScore;
using Core.Shared.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.SeoScore
{
    public class MetaTagModel : BaseModel
    {

        private readonly ISeoScore<string, MetaTag>? seoScore;
        private readonly AzureOpenAiService azureOpenAiService;
        HtmlDocument doc = null;
        /// <summary>
        /// Meta Tags:
        /// Optimized title tags that accurately describe the content of the page.
        /// Compelling and informative meta descriptions that encourage clicks from search engine results.
        /// </summary>
        public MetaTagModel(HtmlDocument document, AzureOpenAiService azureAiService, SeoScoreBase<string, MetaTag> metaTagService) : base(document)
        {

            seoScore = metaTagService;
            azureOpenAiService = azureAiService;
        }

        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string _seedUrl)
        {
            doc = document;
            if (doc != null)
            {
                //doc.Load(htmlDocument);

                var metaTag = new MetaTag
                {
                    CrawledId = crawledId, 
                    Title = GetTitle(doc),
                    Description = GetDescription(doc),
                    Keywords = GetKeywords(doc),
                    Viewport = GetViewport(doc),
                    Charset = GetCharset(doc),
                    Robots = GetRobots(doc),
                    Canonical = GetCanonical(doc),
                    OgTitle = GetOgTitle(doc),
                    OgDescription = GetOgDescription(doc),
                    OgImage = GetOgImage(doc),
                    OgType = GetOgType(doc),
                    OgUrl = GetOgUrl(doc),
                };

                   
                seoScore?.Create("MetaTag", metaTag);
            }
        }
        private string GetTitle(HtmlDocument doc)
        {
            var titleNode = doc.DocumentNode.SelectSingleNode("//title");
            return titleNode?.InnerText.Trim() ?? "Title not found";
        }
        private string GetDescription(HtmlDocument doc)
        {
            var metaDescriptionNode = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            return metaDescriptionNode?.Attributes["content"]?.Value.Trim() ?? "Meta description not found";
        }
        private string GetKeywords(HtmlDocument doc)
        {
            var metaKeywordsNode = doc.DocumentNode.SelectSingleNode("//meta[@name='keywords']");
            return metaKeywordsNode?.Attributes["content"]?.Value.Trim() ?? "Meta keywords not found";
        }
        private string GetViewport(HtmlDocument doc)
        {
            var viewportNode = doc.DocumentNode.SelectSingleNode("//meta[@name='viewport']");
            return viewportNode?.Attributes["content"]?.Value.Trim() ?? "Viewport meta tag not found";
        }
        private string GetCharset(HtmlDocument doc)
        {
            var metaCharsetNode = doc.DocumentNode.SelectSingleNode("//meta[@charset]");
            return metaCharsetNode?.Attributes["charset"]?.Value.Trim() ?? "Charset not found";
        }
        private string GetRobots(HtmlDocument doc)
        {
            var metaRobots = doc.DocumentNode.SelectSingleNode("//meta[@Robots]");
            return metaRobots?.Attributes["Robots"]?.Value.Trim() ?? "Robots not found";
        }
        private string GetCanonical(HtmlDocument doc)
        {
            var metaCanonical = doc.DocumentNode.SelectSingleNode("//Canonical");
            return metaCanonical?.InnerText.Trim() ?? "Title not found";
        }
        private string GetOgTitle(HtmlDocument doc)
        {
            var metaOgTitle = doc.DocumentNode.SelectSingleNode("//meta[@property='og:title']");
            return metaOgTitle?.InnerText.Trim() ?? "Og Title not found";
        }
        private string GetOgDescription(HtmlDocument doc)
        {
            var metaOgDescription = doc.DocumentNode.SelectSingleNode("//meta[@property='og:description']");
            return metaOgDescription?.InnerText.Trim() ?? "Og Description not found";
        }
        private string GetOgImage(HtmlDocument doc)
        {
            var metaOgImage = doc.DocumentNode.SelectSingleNode("//meta[@property='og:image']");
            return metaOgImage?.InnerText.Trim() ?? "OgImage not found";
        }
        private string GetOgType(HtmlDocument doc)
        {
            var metaOgType = doc.DocumentNode.SelectSingleNode("//meta[@property='og:type']");
            return metaOgType?.InnerText.Trim() ?? "OgType not found";
        }
        private string GetOgUrl(HtmlDocument doc)
        {
            var metaOgUrl = doc.DocumentNode.SelectSingleNode("//meta[@property='og:url']");
            return metaOgUrl?.InnerText.Trim() ?? "OgUrl not found";
        }
    }
}
