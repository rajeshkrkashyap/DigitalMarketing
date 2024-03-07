using DAL.Server;
using HtmlAgilityPack;
using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
//using RobotsTxt;
using Serilog;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using SeleniumLib;
using System.Net.Http.Headers;

namespace Server
{
    internal class Crawler
    {
        // URL Discovery
        private readonly CrawledService _crawledService;
        private readonly ContentAnalysis _contentAnalysis;
        private readonly ProjectService _projectService;
        private List<string> SeedUrls { get; set; }
        private string _seedUrl = string.Empty;

        public string SeedUrl
        {
            set
            {
                _seedUrl = value;
                if (_seedUrl.EndsWith("/"))
                {
                    _seedUrl = _seedUrl.TrimEnd('/');
                }
                SeedUrls.Add(_seedUrl);
            }
        }


        // Robots.txt Compliance
        public bool RobotsTxt { get; set; }

        // Politeness
        public int CrawlDelaySeconds { get; set; }

        // Parallelization
        public int MaxConcurrentRequests { get; set; }

        // Content Parsing
        public bool ExtractTextContent { get; set; }
        public bool ExtractLinks { get; set; }
        public List<string> LinkToFilter { get; set; }
        public bool ExtractImages { get; set; }

        // Duplicate Detection
        public HashSet<string> VisitedUrls { get; set; }

        // Link Extraction
        public List<string> ExtractedLinks { get; set; }

        // URL Normalization
        public bool NormalizeUrls { get; set; }

        // URL Filtering and Fingerprinting
        public bool FilterNonHtmlContent { get; set; }
        public bool ApplyUrlFingerprinting { get; set; }

        // Depth-First or Breadth-First Crawling
        public bool UseDepthFirstCrawling { get; set; }

        // Session Handling
        public bool UseSessionHandling { get; set; }

        // Error Handling and Retry Logic
        public int MaxRetries { get; set; }

        // Crawl State Management
        public int MaxVisitedUrls { get; set; }
        public Queue<string> PendingUrls { get; set; }

        // Reporting and Logging
        public bool EnableLogging { get; set; }
        // public Robots robots;
        public string? RobotsTxtContent { get; set; }
        public string? UserAgent { get; set; }

        public List<string?> UrlsToIgnore { get; set; }
        SortedList<string?, string?> UrlSortedList { get; set; }

        Uri baseUri = null;
        string? _projectId;

        private void SetProjectId()
        {

            var project = _projectService.GetProjectId(_seedUrl);
            _projectId = project.Id;

            if (!string.IsNullOrEmpty(_projectId))
            {
                _projectService.ProjectUpdateStatus(_projectId, "Processing");
            }
        }

        // Constructor
        public Crawler(ProjectService projectService, CrawledService crawledService, ContentAnalysis contentAnalysis, LoggerConfiguration logger)
        {
            SeedUrls = new List<string>();
            VisitedUrls = new HashSet<string>();
            ExtractedLinks = new List<string>();
            PendingUrls = new Queue<string>();
            LinkToFilter = new List<string>();
            UrlSortedList = new SortedList<string, string>();
            MaxRetries = 1;
            EnableLogging = true;
            //string robotsTxtContent = "User-Agent: *\nDisallow: /private/\nAllow: /public/\n";
            // robots = new Robots(RobotsTxtContent);
            Serilog.Log.Logger = logger.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day).CreateLogger();
            _projectService = projectService;
            _crawledService = crawledService;
            _contentAnalysis = contentAnalysis;
        }

        //private long CrawlDelay()
        //{
        //    return robots.CrawlDelay(UserAgent);
        //}
        private bool IsUrlAllowedByRobotsTxt(string url)
        {
            //if (UserAgent != null && url != null)
            //    return robots.IsPathAllowed(UserAgent, url);//URL example:  "/public/page.html"
            return true;
        }
        public string? Crawl()
        {
            //Get the base domain of the current page
            baseUri = new Uri(_seedUrl); //Replace with the actual base URL of the page
            SetProjectId();
            if (string.IsNullOrEmpty(_projectId))
            {
                Console.WriteLine("ProjectURL does not exist!");
                return _projectId;
            }

            foreach (string url in SeedUrls)
            {
                PendingUrls.Enqueue(url);
            }

            while (PendingUrls.Count > 0)
            {
                string url = PendingUrls.Dequeue();
                if (!VisitedUrls.Contains(url) && IsUrlAllowedByRobotsTxt(url))
                {
                    int retries = 0;
                    while (retries <= MaxRetries)
                    {
                        try
                        {
                            string htmlContent = DownloadHtmlContent(url);
                            VisitedUrls.Add(url);
                            ExtractLinksFromHtml(htmlContent);
                            _ = AddUpdateToDatabase(url, htmlContent);
                            Logger($"\nCrawled: {url} ");

                            Console.Write(DateTime.UtcNow + $"\n Crawled: {url}  ");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Logger($"\nError crawling {url}: {ex.Message}");
                            retries++;
                        }
                    }
                }
            }

            Log.CloseAndFlush();

            var pageCount = VisitedUrls.Count();

            Console.Write(baseUri.Host + $"\n Crawling Finished!");
            Console.Write($"\nWaiting for another website URL!");

            return _projectId;
        }

        private string DownloadHtmlContent(string url)
        {
            //using (HttpClient client = new HttpClient())
            //{
            //    return client.GetStringAsync(url).Result;
            //}
            return WebDocument.DownloadPageSource(url);
        }
        HtmlDocument doc = null;
        private void ExtractLinksFromHtml(string htmlContent)
        {
            doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // Perform actions on the HTML content
            // Example: Extract all links

            var links = doc.DocumentNode.SelectNodes("//a[@href]")
                                        .Where(i => !LinkToFilter.Contains(i.Attributes["href"].Value)
                                                && !i.Attributes["href"].Value.Contains("/login")
                                                && !i.Attributes["href"].Value.Contains("/register")).ToList();

            if (links != null)
            {
                foreach (var link in links)
                {
                    LinkToFilter.Add(link.Attributes["href"].Value);
                    string hrefValue = link.GetAttributeValue("href", "");

                    // Check if the link is an absolute URL
                    Uri uri;
                    if (Uri.TryCreate(hrefValue, UriKind.Absolute, out uri))
                    {
                        // Check if the domain matches the base domain
                        if (uri.Host == baseUri.Host || uri.Host == "www." + baseUri.Host)
                        {
                            var url = link.Attributes["href"].Value;
                            if (IsUrlAddedInList(url))
                            {
                                PendingUrls.Enqueue(url);
                            }
                        }
                    }
                    else
                    {
                        // This link is a relative URL, you may want to process it accordingly
                        string url = "";
                        if (_seedUrl.EndsWith("/"))
                        {
                            url = _seedUrl + link.Attributes["href"].Value;
                        }
                        else
                        {
                            url = _seedUrl + "/" + link.Attributes["href"].Value;
                        }

                        if (IsUrlAddedInList(url))
                        {
                            PendingUrls.Enqueue(url);
                        }
                        // ...
                    }
                }
            }


        }
        private bool IsUrlAddedInList(string url)
        {
            try
            {
                if (UrlSortedList.ContainsKey(url))
                {
                    Logger($"IgnoreURL: {url}");
                    return false;
                }
                foreach (string pattern in UrlsToIgnore)
                {
                    var value = Regex.Matches(url, pattern, RegexOptions.IgnoreCase);
                    if (value.Count > 0)
                    {
                        Logger($"IgnoreURL: {url}");
                        return false;
                    }
                }
                if (IsJavaScriptLink(url)
                    || IsStylesheetLink(url)
                    || IsImageLink(url)
                    || IsFileLink(url)
                    //|| IsErrorPage(url)
                    || IsSocialMediaLink(url)
                    || HasSessionOrTrackingParams(url))
                {
                    Logger($"IgnoreURL: {url}");
                    return false;
                }

                UrlSortedList.Add(url, url);
                return true;
            }
            catch (Exception)
            {
                Logger($"IgnoreURL: {url}");
                return false;
            }
        }


        private async Task AddUpdateToDatabase(string url, string htmlContent)
        {
            var crawled = new Crawled
            {
                ProjectId = _projectId,
                URL = url,
                PageContent = htmlContent,
                AnalysisStatus = "InProgress"
            };

            if (await _crawledService.Create(crawled))
            {
                try
                {
                    await _contentAnalysis.ExtractAllInformationAsync(_seedUrl, htmlContent, crawled.Id);
                    var updateCrawled = new Crawled
                    {
                        Id = crawled.Id,
                        AnalysisStatus = "Completed"
                    };
                    await _crawledService.UpdateAnalysisStatus(updateCrawled);
                    _projectService.ProjectUpdatePageCount(_projectId, VisitedUrls.Count());

                }
                catch (Exception ex)
                {
                    var updateCrawled = new Crawled
                    {
                        Id = crawled.Id,
                        AnalysisStatus = "Failed"
                    };
                    await _crawledService.UpdateAnalysisStatus(updateCrawled);
                }
            }

        }
        private void Logger(string message)
        {
            if (EnableLogging)
            {
                Log.Information(message);
            }
        }

        #region Ignore URL Functions
        static bool IsJavaScriptLink(string url)
        {
            return url.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase);
        }
        static bool IsStylesheetLink(string url)
        {
            string[] stylesheetExtensions = { ".css", ".scss", ".less" }; // Add more extensions if needed
            foreach (var extension in stylesheetExtensions)
            {
                if (url.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        static bool IsImageLink(string url)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg" }; // Add more extensions if needed
            foreach (var extension in imageExtensions)
            {
                if (url.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        static bool IsFileLink(string url)
        {
            string[] fileExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" }; // Add more extensions if needed
            foreach (var extension in fileExtensions)
            {
                if (url.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        static bool IsErrorPage(string url)
        {
            try
            {
                //var request = (HttpWebRequest)WebRequest.Create(url);
                //request.Method = "HEAD";

                //using (var response = (HttpWebResponse)request.GetResponse())
                //{
                //    return response.StatusCode >= HttpStatusCode.BadRequest; // Check for error status codes
                //}
                return false;
            }
            catch (WebException ex)
            {
                return true; // WebException indicates an error
            }
        }
        static bool IsSocialMediaLink(string url)
        {
            List<string> socialMediaDomains = new List<string>
        {
            "twitter.com",
            "facebook.com",
            "instagram.com",
            // Add more social media domains as needed
        };

            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                return socialMediaDomains.Any(domain => uri.Host.EndsWith(domain, StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }
        static bool HasSessionOrTrackingParams(string url)
        {
            Uri uri = new Uri(url);

            // List of query parameters that indicate session or tracking information
            List<string> sessionTrackingParams = new List<string>
        {
            "sessionID",
            "utm_source",
            "tracking",
            // Add more parameters as needed
        };

            foreach (string param in uri.Query.TrimStart('?').Split('&'))
            {
                string paramName = param.Split('=')[0];
                if (sessionTrackingParams.Contains(paramName))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
