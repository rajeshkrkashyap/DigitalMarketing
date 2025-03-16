using Core.Service.Azure;
using Core.Service.SeoScore;
using Core.Shared.Entities;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.SeoScore
{
    public class SocialSignalModel : BaseModel
    {
        HtmlDocument doc = null;
        private readonly AzureOpenAiService azureOpenAiService;
        private readonly ISeoScore<string, SocialSignal>? socialSignalService;
        public SocialSignalModel(HtmlDocument document, AzureOpenAiService azureAiService,
            SeoScoreBase<string, SocialSignal> socialSignal) : base(document)
        {
            azureOpenAiService = azureAiService;
            socialSignalService = socialSignal;
        }

        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string seedUrl = null)
        {
            doc= document;
            if (doc != null)
            {
               // doc.Load(htmlDocument);
                try
                {
                    SocialSignal socialSignal = new SocialSignal
                    {
                        CrawledId = crawledId,
                        AnalyzeSocialSignals = AnalyzeSocialSignals(),
                        IsActive = true,
                    };
                    if (socialSignalService != null)
                    {
                        socialSignalService.Create("SocialSignal", socialSignal);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private string AnalyzeSocialSignals()
        {
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();

            string[] socialMedias = new[] { "Facebook", "Twitter", "LinkedIn", "Pinterest" };

            // Define XPath or CSS selectors for each social media platform's engagement metrics
            Dictionary<string, string> socialMediaSelectors = new Dictionary<string, string>
            {
                { "Facebook", "//meta[@property='og:share_count']" },
                { "Twitter", "//meta[@name='twitter:mentions']" },
                { "LinkedIn", "//meta[@name='linkedin:shares']" },
                { "Pinterest", "//meta[@property='pinterest:pin_count']" },
                // Add more social media platforms and their respective selectors as needed
            };

            foreach (var socialMedia in socialMedias)
            {
                // Check if the specified social media platform is supported
                if (socialMediaSelectors.ContainsKey(socialMedia))
                {
                    // Extract engagement metric for the specified social media platform
                    string selector = socialMediaSelectors[socialMedia];

                    HtmlNode socialMediaNode = doc.DocumentNode.SelectSingleNode(selector);
                    if (socialMediaNode != null)
                    {
                        int engagementMetric = int.Parse(socialMediaNode.GetAttributeValue("content", "0"));
                        Console.WriteLine($"{socialMedia} {engagementMetric}");
                        keyValuePairs.Add(socialMedia, engagementMetric);
                    }
                    else
                    {
                        Console.WriteLine($"No engagement metrics found for {socialMedia}");
                        keyValuePairs.Add(socialMedia, 0);
                    }
                }
                else
                {
                    Console.WriteLine($"Social media platform '{socialMedia}' is not supported.");
                    keyValuePairs.Add(socialMedia, -1);
                }
            }
            return JsonConvert.SerializeObject(keyValuePairs, Formatting.Indented);
        }
    }
}
