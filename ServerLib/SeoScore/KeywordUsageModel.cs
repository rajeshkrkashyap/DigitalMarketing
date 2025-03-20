using Core.Shared.Entities;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Service.OnPageSeoScore;
namespace ServerLib.SeoScore
{
    public class KeywordUsageModel : BaseModel
    {
        ISeoScore<string, List<KeywordUsage>> seoScore;
        HtmlDocument doc = null;
        public KeywordUsageModel(HtmlDocument document, SeoScoreBase<string, List<KeywordUsage>> SeoScore) : base(document)
        {
            KeywordUsage = new KeywordUsage();
            seoScore = SeoScore;
        }

        /// <summary>
        /// Keyword Usage:
        /// Presence of target keywords in the title tag, meta description, heading tags (H1, H2, etc.), 
        /// and throughout the content.
        /// Keyword density, ensuring a natural and appropriate distribution of keywords within the content.
        /// </summary>
        public KeywordUsage? KeywordUsage { get; set; }

        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string _seedUrl)
        {
            doc = document;
            if (doc != null)
            {
                try
                {

                    //doc.Load(htmlDocument);
                }
                catch (Exception ex)
                {

                    throw;
                }

                List<string> keywords = ExtractMetaTagKeyword(ignoreWordList, htmlDocument, crawledId);
                if (keywords != null)
                {
                    List<KeywordUsage> keywordUsageList = new List<KeywordUsage>();
                    foreach (string keyword in keywords)
                    {
                        var keywordsInContent = TotalKeywordsInContent(keyword);
                        var keywordsInMetaDescription = TotalKeywordsInMetaDescription(keyword);
                        var keywordsInHeading = TotalKeywordsInHeading(keyword);
                        var keywordsInTitle = TotalKeywordsInTitle(keyword);

                        var isMainKeyword = IsMainKeyword(keywordsInContent, keywordsInMetaDescription, keywordsInHeading, keywordsInTitle);

                        keywordUsageList.Add(new KeywordUsage()
                        {
                            CrawledId = crawledId,
                            Keyword = keyword,
                            KeywordsInContent = keywordsInContent,
                            KeywordsInMetaDescription = keywordsInMetaDescription,
                            KeywordsInHeading = keywordsInHeading,
                            KeywordsInTitle = keywordsInTitle,
                            IsMainKeyword = isMainKeyword,
                            IsActive = true,
                            Created = DateTime.UtcNow,
                            Updated = DateTime.UtcNow,
                        });
                    }

                    if (keywordUsageList != null && keywordUsageList.Count() > 0)
                    {
                        seoScore.CreateList("KeywordUsage", keywordUsageList);
                    }


                }
            }
        }

        private static bool IsMainKeyword(int keywordsInContent, int keywordsInMetaDescription, int keywordsInHeading, int keywordsInTitle)
        {
            bool isMainKeyword = false;
            if (keywordsInContent > 0 && keywordsInMetaDescription > 0 && keywordsInHeading > 0 && keywordsInTitle > 0)
            {
                isMainKeyword = true;
            }
            return isMainKeyword;
        }

        private int TotalKeywordsInTitle(string keyword)
        {
            if (doc == null && keyword == null)
            {
                return 0;
            }

            // Select nodes containing main content. This can vary based on the structure of the webpage.
            string title = doc.DocumentNode.SelectSingleNode("//title")?.InnerText ?? "";

            if (title != null)
            {
                // Count occurrences of the keyword using Regex.Matches
                int keywordCount = Regex.Matches(title, keyword, RegexOptions.IgnoreCase).Count;

                Console.WriteLine($"Total occurrences of keyword '{keyword}': {keywordCount}");
                return keywordCount;
            }
            else
            {
                Console.WriteLine("Main content nodes not found.");
            }
            return 0;
        }

        private int TotalKeywordsInHeading(string keyword)
        {
            if (doc == null && keyword == null)
            {
                return 0;
            }

            // Select nodes containing main content. This can vary based on the structure of the webpage.
            var headings = doc.DocumentNode.SelectNodes("//h1 | //h2 | //h3 | //h4 | //h5 | //h6");

            if (headings != null)
            {
                // Count occurrences of the keyword using Regex.Matches
                int keywordCount = headings != null ? headings.Sum(heading => Regex.Matches(heading.InnerText, keyword, RegexOptions.IgnoreCase).Count) : 0;

                Console.WriteLine($"Total occurrences of keyword '{keyword}': {keywordCount}");
                return keywordCount;
            }
            else
            {
                Console.WriteLine("Main content nodes not found.");
            }

            return 0;
        }

        private int TotalKeywordsInMetaDescription(string keyword)
        {
            if (doc == null && keyword == null)
            {
                return 0;
            }

            // Select nodes containing main content. This can vary based on the structure of the webpage.
            string metaDescription = doc.DocumentNode.SelectSingleNode("//meta[@name='description']")?.Attributes["content"]?.Value ?? "";

            if (metaDescription != null)
            {
                // Count occurrences of the keyword using Regex.Matches
                int keywordCount = Regex.Matches(metaDescription, keyword, RegexOptions.IgnoreCase).Count;

                Console.WriteLine($"Total occurrences of keyword '{keyword}': {keywordCount}");
                return keywordCount;
            }
            else
            {
                Console.WriteLine("Main content nodes not found.");
            }

            return 0;
        }

        private int TotalKeywordsInContent(string keyword)
        {
            if (doc == null && keyword == null)
            {
                return 0;
            }

            // Select nodes containing main content. This can vary based on the structure of the webpage.
            HtmlNodeCollection mainContentNodes = doc.DocumentNode.SelectNodes("//div[@id='main-content'] | //article | //div[@class='content']");

            if (mainContentNodes != null)
            {
                // Concatenate inner text of all selected nodes
                string mainContent = string.Join(" ", mainContentNodes.Select(node => node.InnerText));

                // Count occurrences of the keyword using Regex.Matches
                int keywordCount = Regex.Matches(mainContent, keyword, RegexOptions.IgnoreCase).Count;

                Console.WriteLine($"Total occurrences of keyword '{keyword}': {keywordCount}");
                return keywordCount;
            }
            else
            {
                Console.WriteLine("Main content nodes not found.");
            }

            return 0;
        }

        private List<string> ExtractMetaTagKeyword(List<string> ignoreWordList, string htmlDocument, string crawledId)
        {
            if (doc == null && ignoreWordList == null)
            {
                return null;
            }

            HtmlNodeCollection keywordNodes = doc.DocumentNode.SelectNodes("//meta[@name='keywords']");
            List<string> keywords = new List<string>();
            if (keywordNodes != null)
            {
                foreach (HtmlNode keywordNode in keywordNodes)
                {
                    string keyword = keywordNode.GetAttributeValue("content", "");
                    keywords.Add(keyword);
                }

                return keywords.Where(k => !ignoreWordList.Contains(k.ToLower())).ToList();
            }

            return null;
        }
    }
}
