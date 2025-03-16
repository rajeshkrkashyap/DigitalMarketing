using ConnectToAi.Areas.Marketing.Models;
using ConnectToAi.Services;
using Core.Shared;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class AnalysingController : BaseController
    {
        public AnalysingController(ConfigService configService) : base(configService)
        {

        }
        public IActionResult Index()
        {
            if (UserDetail!=null)
            {
                using (ProjectService projectService = new(_configService))
                {
                    var projects = projectService.ListAsync(UserDetail.UserID).Result;

                    if (projects.Count()>0)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }

            }
            return View(new ProjectViewModel());
        }
        public async Task<IActionResult> AddProject(ProjectViewModel projectViewModel)
        {
            using (ProjectService projectService = new(_configService))
            {
                var addProject = new Project
                {
                    Name = projectViewModel.Name,
                    Description = projectViewModel.Description,
                    URL = projectViewModel.URL,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    WhoIsDomain = GetWhoIsDomain(projectViewModel.URL),
                    IsMalicious = await IsMalicious(projectViewModel.URL),
                    HasValidSSL = await HasValidSSL(projectViewModel.URL),
                    IsBlacklisted = await IsBlacklisted(projectViewModel.URL),
                    IsActive = true,
                    AnalysisStatus = "Start",
                    AppUserId = UserDetail.UserID
                };

                var project = projectService.CreateAsync(addProject).Result;

                if (!string.IsNullOrEmpty(project.URL))
                {
                    SendProjectForAnalysis(project.Id);
                }
            }

            return RedirectToAction("Index", "Dashboard");
        }


        #region  Check domain reputation 
        static async Task<bool> IsMalicious(string domain)
        {
            // Implement logic to check for malware using online scanning services or APIs
            // For simplicity, return false (not malicious)
            string apiUrl = $"https://www.virustotal.com/api/v3/domains/{domain}";

            // Replace "YOUR_API_KEY" with your actual API key obtained from VirusTotal
            string apiKey = "893cd8f344ef47d4d9ba785923f00705f993e035ef58317984e007d935c76f4e";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-apikey", apiKey);

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response to determine if the domain is flagged as malicious
                    // This depends on the specific format of the response from the service
                    // Example: Check for specific indicators in the response JSON
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response to check if the domain is flagged as malicious
                    // Example: var isMalicious = ParseResponse(responseBody);
                    return responseBody.Contains("malicious");
                }
            }
            return false;
        }
        static async Task<bool> HasValidSSL(string domain)
        {
            try
            {
                // Attempt to establish an HTTPS connection to the domain
                WebClient client = new();
                client.DownloadString($"https://{domain}");
                return true; // If successful, the domain has a valid SSL certificate
            }
            catch (Exception)
            {
                return false; // If an error occurs, the domain does not have a valid SSL certificate
            }
        }
        static async Task<bool> IsBlacklisted(string domain)
        {
            // Implement logic to check if domain is blacklisted using online reputation services or APIs
            // For simplicity, return false (not blacklisted)
            string apiUrl = $"https://www.spamhaus.org/query/domain/{domain}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Check if the response body indicates that the domain is listed on the Spamhaus DBL
                    return responseBody.Contains("DOMAIN LISTED IN DBL");
                }
            }
            return false;
        }
        static string? GetWhoIsDomain(string domain)
        {
            // Implement logic to fetch domain registration date using WHOIS lookup or domain registrar APIs
            // For simplicity, return a random registration date within the last 5 years
            if (!string.IsNullOrEmpty(domain))
            {

                //https://user.whoisxmlapi.com/products
                string apiKey = "at_qd1IDzh5eKizOI1FA23v7ALxdHrTj";

                string url = "https://www.whoisxmlapi.com/whoisserver/WhoisService?"
                             + "domainName=" + domain
                             + "&apiKey=" + apiKey
                             + "&outputFormat=" + "JSON";

                try
                {
                    // Download JSON into a dynamic object
                    dynamic result = new System.Net.WebClient().DownloadString(url);

                    // Print a nice informative string
                    Console.WriteLine("JSON:\n");
                    Console.WriteLine(result);
                    return result.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine("An unkown error has occurred!");
                }

            }

            return null;
        }
        #endregion

    }
}
