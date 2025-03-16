using Core.Service.Server;
using Core.Shared.Entities;
using HtmlAgilityPack;
using ServerLib.SeoScore;
using System.Reflection.Metadata;
using System.Text;

namespace ServerLib
{
    public class BaseModel : IOnPageSeoScore
    {
        protected readonly HtmlDocument? doc1 = null;
        protected readonly CrawledService? crawledService = null;
        public string Domain { get; set; }
        public BaseModel(HtmlDocument document)
        {
            doc1 = document;
        }

        public BaseModel(HtmlDocument document, CrawledService crawler)
        {
            doc1 = document;
            crawledService = crawler;
        }
        public virtual void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string seedUrl = null)
        {
            throw new NotImplementedException();
        }

        #region  Check domain reputation 
        protected async Task<double> EvaluateReputation(Project project, HtmlDocument doc)
        {
            // This method can include logic to evaluate the reputation of sources mentioned on the webpage.
            // For example, checking the domain reputation, credibility of organizations mentioned, etc.
            // For simplicity, let's assume a random reputation score between 0 and 1.

            //DateTime domainRegistrationDate = GetDomainRegistrationDate(Domain);

            bool isMalicious = project.IsMalicious; // await IsMalicious(Domain);
            bool hasValidSSL = project.HasValidSSL;//await HasValidSSL(Domain);
            bool isBlacklisted = project.IsBlacklisted;//await IsBlacklisted(Domain);

            if (isMalicious && hasValidSSL && isBlacklisted)
            {
                return 1;
            }
            return 0;
        }

        protected int GetDomainAge(string domain)
        {
            // Example logic to fetch domain age from WHOIS lookup or API
            // Return hypothetical domain age in years
            return 5;  // Example: Domain age of 5 years
        }

        protected List<string> GetBacklinkCount(string domain)
        {
            // Example logic to fetch backlink count from SEO tool API
            // Return hypothetical backlink count
            return new List<string>();  // Example: 100 backlinks
        }

        protected int GetSocialEngagement(string domain)
        {
            // Example logic to fetch social engagement metrics from social media APIs
            // Return hypothetical social engagement score
            return 80;  // Example: Social engagement score of 80 (out of 100)
        }

        protected double GetAuthorExpertise(string author)
        {
            // Example logic to evaluate author expertise based on credentials and experience
            // Return hypothetical expertise score
            return 0.8;  // Example: Author expertise score of 0.8 (on a scale of 0 to 1)
        }
        #endregion

        protected static double CalculateAccuracyCredibilityScore(bool hasAuthoritativeSources, bool hasCitations, double reputationScore)
        {
            // Calculate the accuracy and credibility score based on the presence of authoritative sources,
            // citations, and the reputation of sources.
            // You can define your scoring algorithm based on specific criteria and weightage.
            // For simplicity, let's calculate a weighted average score.
            double weightageAuthoritativeSources = 0.4;
            double weightageCitations = 0.3;
            double weightageReputation = 0.3;

            return (hasAuthoritativeSources ? 1 : 0) * weightageAuthoritativeSources +
                   (hasCitations ? 1 : 0) * weightageCitations +
                   reputationScore * weightageReputation;
        }

        protected static async Task UXfunction()
        {
            string apiKey = "YOUR_API_KEY";
            string apiUrl = "https://chromeuxreport.googleapis.com/v1/records:queryRecord?key=" + apiKey;

            // Create HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create JSON data
                    var postData = new
                    {
                        formFactor = "PHONE",
                        origin = "https://www.example.com",
                        metrics = new string[] { "largest_contentful_paint", "experimental_time_to_first_byte" }
                    };

                    string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(postData);

                    // Create HttpContent
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Make POST request
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Check response status
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent); // Print response content
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        protected static int ContainsKeywords(string content, string[] keywords)
        {
            content = content.ToLower(); // Convert to lowercase for case-insensitive matching
            int count = 0;
            foreach (string keyword in keywords)
            {
                if (content.Contains(keyword.ToLower()))
                {
                    count++;
                }
            }

            return count;
        }
    }
}