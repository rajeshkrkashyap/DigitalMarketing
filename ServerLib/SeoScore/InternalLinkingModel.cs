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
    public class InternalLinkingModel : BaseModel
    {
        HtmlDocument doc = null;
        private readonly AzureOpenAiService azureOpenAiService;
        private readonly ISeoScore<string, InternalLinking>? internalLinkingService;
        public InternalLinkingModel(HtmlDocument document, AzureOpenAiService azureAiService,
            SeoScoreBase<string, InternalLinking> internalLinking) : base(document)
        {
            azureOpenAiService = azureAiService;
            internalLinkingService = internalLinking;
        }

        /// <summary>
        /// Internal Linking:
        /// Effective use of internal links to connect related pages within the website. Anchor text that 
        /// provides context for the linked content.
        /// </summary>
        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string _seedUrl)
        {doc = document;
            if (doc != null)
            {
                //doc.Load(htmlDocument);
                var anchorNodes = doc.DocumentNode.SelectNodes("//a");
                if (anchorNodes != null)
                {
                    foreach (var anchorNode in anchorNodes)
                    {
                        // Extract href attribute value
                        string href = anchorNode.GetAttributeValue("href", "");
                        var isInternal = false;
                        if (href.StartsWith("/"))
                        {
                            isInternal = true;
                        }

                        try
                        {
                            InternalLinking internalLinking = new InternalLinking
                            {
                                CrawledId = crawledId,
                                LinkText = anchorNode.InnerText.Trim(),
                                Link = href,
                                IsInternal = isInternal,
                                IsActive = true
                            };

                            internalLinkingService.Create("InternalLinking", internalLinking);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }
    }
}
