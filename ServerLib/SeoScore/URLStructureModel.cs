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
    public class URLStructureModel : BaseModel
    {
        HtmlDocument doc = null;
        private readonly AzureOpenAiService azureOpenAiService;
        private readonly ISeoScore<string, URLStructure>? uRLStructureService;
        public URLStructureModel(HtmlDocument document, AzureOpenAiService azureAiService,
            SeoScoreBase<string, URLStructure> uRLStructure) : base(document)
        {
            azureOpenAiService = azureAiService;
            uRLStructureService = uRLStructure;
        }

        /// <summary>
        /// URL Structure:
        /// Clean and descriptive URLs that include relevant keywords. Avoidance of unnecessary parameters, 
        /// symbols, or numbers in URLs.
        /// </summary>


        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string seedUrl)
        {
            doc= document;
            if (doc != null)
            {
                //doc.Load(htmlDocument);

                bool isCleanURL = IsCleanURL(seedUrl);
                try
                {
                    URLStructure uRLStructure = new URLStructure
                    {
                        CrawledId = crawledId,
                        URL = seedUrl,
                        IsCleanURL = isCleanURL,
                        IsActive = true
                    };

                    uRLStructureService.Create("URLStructure", uRLStructure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        static bool IsCleanURL(string url)
        {
            // Criteria for clean URL:
            // 1. Contains relevant keywords
            // 2. Avoids unnecessary parameters, symbols, or numbers

            // Check if URL contains parameters
            bool hasParameters = url.Contains("?");

            // Check if URL contains numbers or symbols in the path
            bool hasNumbersOrSymbols = ContainsNumbersOrSymbols(url);

            // Determine if the URL meets the criteria
            return !hasParameters && !hasNumbersOrSymbols;
        }

        static bool ContainsNumbersOrSymbols(string url)
        {
            // Define symbols and numbers that are not allowed in the URL path
            char[] disallowedChars = { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '{', '}', '[', ']', '|', '\\', ':', ';', '"', '\'', '<', '>', ',', '.', '?', '/', ' ', '\t' };

            // Check if the URL path contains any disallowed characters
            foreach (char ch in url)
            {
                if (disallowedChars.Contains(ch))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
