using Core.Service.Azure;
using Core.Service.OnPageSeoScore;
using Core.Shared.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.SeoScore
{
    public class PageLoadingSpeedModel : BaseModel
    {
        HtmlDocument doc = null;
        private readonly AzureOpenAiService azureOpenAiService;
        private readonly ISeoScore<string, PageLoadingSpeed>? pageLoadingSpeedService;
        public PageLoadingSpeedModel(HtmlDocument document, AzureOpenAiService azureAiService,
            SeoScoreBase<string, PageLoadingSpeed> pageLoadingSpeed) : base(document)
        {
            azureOpenAiService = azureAiService;
            pageLoadingSpeedService = pageLoadingSpeed;
        }
        /// <summary>
        /// Page Loading Speed:
        /// Fast page loading times for a positive user experience.
        /// Optimization of images, code, and server response times to improve speed.
        /// </summary>

        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string seedURL)
        {
            doc = document;
            if (doc != null)
            {
                //doc.Load(htmlDocument);
                try
                {
                    PageLoadingSpeed pageLoadingSpeed = new PageLoadingSpeed
                    {
                        CrawledId = crawledId,
                        URL = seedURL,
                        PageLoadTime = GetPageLoadTime(seedURL),
                        IsActive = true
                    };

                     pageLoadingSpeedService.Create("PageLoadingSpeed", pageLoadingSpeed);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private TimeSpan? GetPageLoadTime(string seedURL)
        {
            // Create a new instance of the Chrome driver
            return SeleniumLib.WebDocument.MeasurePageLoadingSpeed(seedURL);
        }      
    }
}
