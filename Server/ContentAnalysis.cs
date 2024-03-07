using DAL.Server;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace Server
{
    public class ContentAnalysis
    {
        private string _baseUrl = "";
        private readonly HtmlDocument document;
        private readonly ContentAnalysisService _contentAnalysisService;
        private readonly List<string>? _ignoreWordList;
        public ContentAnalysis(List<string>? ignoreWordList, ContentAnalysisService contentAnalysisService)
        {
            document = new HtmlDocument();
            _contentAnalysisService = contentAnalysisService;
            _ignoreWordList = ignoreWordList;
        }

        public async Task ExtractAllInformationAsync(string baseURL, string htmlContent, string crawledId)
        {
            _baseUrl = baseURL;
            document.LoadHtml(htmlContent);
            var keywordsInJson = ExtractMetaTagKeyword();
            var keywordFrequency = ExtractKeywordFromContentWithTheirFrequency();
            var headings = ExtractHeadingSubheadings();
            var title = ExtractTitle();
            var metaDescription = ExtractMetaDescription();
            var imageDetail = ExtractImageDetail();
            var internalLinks = ExtractInternalLinks();
            var externalLinks = ExtractExternalLinks();
            var uRLStructure = URLStructure();

            await _contentAnalysisService.Create(new Core.Shared.Entities.ContentAnalysis
            {
                CrawledId = crawledId,
                MetaTagKeywords = keywordsInJson,
                Headings = headings,
                KeywordFrequency = keywordFrequency,
                MetaDescription = metaDescription,
                Title = title,
                ImageDetail = imageDetail,
                InternalLinks = internalLinks,
                ExternalLinks = externalLinks,
                URLStructure = uRLStructure
            });
        }
        private string ExtractMetaTagKeyword()
        {
            if (_ignoreWordList == null)
            {
                return null;
            }

            HtmlNodeCollection keywordNodes = document.DocumentNode.SelectNodes("//meta[@name='keywords']");
            List<string> keywords = new List<string>();
            if (keywordNodes != null)
            {
                foreach (HtmlNode keywordNode in keywordNodes)
                {
                    string keyword = keywordNode.GetAttributeValue("content", "");
                    keywords.Add(keyword);
                }

                var filteredKeywords = keywords.Where(k => !_ignoreWordList.Contains(k.ToLower()));

                return JsonConvert.SerializeObject(filteredKeywords);
            }
            return JsonConvert.SerializeObject(null);
        }
        private string ExtractKeywordFromContentWithTheirFrequency()
        {
            if (_ignoreWordList == null)
            {
                return null;
            }

            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//p");
            List<string> keywords = new List<string>();

            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    string[] words = node.InnerHtml.Split(' ');
                    foreach (string word in words)
                    {
                        if (!string.IsNullOrWhiteSpace(word))
                        {
                            if (word.Length > 2)
                            {
                                keywords.Add(word);
                            }
                        }
                    }
                }
            }


            var filteredKeywords = keywords.Where(k => !_ignoreWordList.Contains(k.ToLower())).ToList();


            //Getting frequency of the each words
            var frequency = filteredKeywords.GroupBy(x => x)
                                     .Where(x => x.Count() > 3)
                                     .Select(x => new { Keyword = x.Key, Count = x.Count() });

            return JsonConvert.SerializeObject(frequency);
        }
        private string ExtractHeadingSubheadings()
        {
            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//h1 | //h2 | //h3 | //h4 | //h5 | //h6");
            List<string> headingList = new List<string>();

            if (nodes != null)
            {
                headingList = nodes.Select(s => s.OuterHtml).ToList();
            }

            //Getting frequency of the each words
            var headingFrequency = headingList.GroupBy(x => x)
                                     .Select(x => new { Heading = x.Key, Count = x.Count() });

            return JsonConvert.SerializeObject(headingFrequency);
        }
        private string ExtractTitle()
        {
            HtmlNode titleNode = document.DocumentNode.SelectSingleNode("//title");
            if (titleNode!=null)
            {
                return titleNode.InnerText;
            }
            return "";
        }
        private string ExtractMetaDescription()
        {
            HtmlNode metaDescriptionNode = document.DocumentNode.SelectSingleNode("//meta[@name='description']");
            if (metaDescriptionNode != null)
            {
                return metaDescriptionNode.GetAttributeValue("content", "");
            }
            return null;

            //Unique Descriptions: Each page on your website should have a unique MetaDescription.
            //Avoid using identical descriptions for multiple pages, as this can confuse search engines.

            //Length Consideration: While there is no strict character limit for MetaDescriptions, it's
            //generally recommended to keep them between 150-160 characters. This ensures that the description is
            //displayed properly in search results.

            //Avoid Duplicating Content: Ensure that the MetaDescription does not simply repeat the title of the page.
            //It should complement the title by providing additional context.
        }
        private string ExtractImageDetail()
        {

            HtmlNodeCollection imageNodes = document.DocumentNode.SelectNodes("//img");


            List<Img> imageList = new List<Img>();

            if (imageNodes != null)
            {

                foreach (var imageNode in imageNodes)
                {
                    imageList.Add(new Img()
                    {
                        FileName = imageNode.GetAttributeValue("src", ""),
                        AltTag = imageNode.GetAttributeValue("alt", "")
                    });
                }
            }
            return JsonConvert.SerializeObject(imageList);
        }
        private string ExtractInternalLinks()
        {
            var anchorElements = document.DocumentNode.SelectNodes("//a[@href]");

            foreach (var anchorElement in anchorElements)
            {
                string href = anchorElement.GetAttributeValue("href", "");
                // Process the href value as needed
            }

            var internalLinks = anchorElements
                .Select(a => a.GetAttributeValue("href", ""))
                .Where(href => href.StartsWith("/") || href.StartsWith(_baseUrl));

            //Getting frequency of the each words
            var internalLinksAndFrequency = internalLinks.GroupBy(x => x)
                                     .Select(x => new { Keyword = x.Key, Count = x.Count() });

            return JsonConvert.SerializeObject(internalLinksAndFrequency);
        }
        private string ExtractExternalLinks()
        {
            var links = document.DocumentNode.Descendants("a")
                                                     .Where(a => a.Attributes["href"] != null &&
                                                                 (a.Attributes["href"].Value.StartsWith("http://") ||
                                                                  a.Attributes["href"].Value.StartsWith("https://")))
                                                     .Select(a => a.Attributes["href"].Value);
            if (links != null && links.Count() > 0)
            {
                Uri uri = new Uri(_baseUrl);
                var host = uri.Host.Replace("www.", "");
                var externalLinks = links.Where(l => !l.Contains(host));
                return JsonConvert.SerializeObject(externalLinks);
            }
            return null;
        }
        private string URLStructure()
        {
            URLStructure uRLStructure = new URLStructure();
            Uri uri = new Uri(_baseUrl);
            uRLStructure.Scheme = uri.Scheme;  // "https"
            uRLStructure.Host = uri.Host;      // "www.example.com"
            uRLStructure.AbsolutePath = uri.AbsolutePath;  // "/page"
            uRLStructure.Query = uri.Query;    // "?param=value"


            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                // Invalid scheme
                uRLStructure.isSchemeValid = "Invalid";
            }
            //if (!Regex.IsMatch(uri.Host, @"^(www\.)?example\.com$"))
            //{
            //    uRLStructure.isHostValid = "Invalid";
            //    // Invalid host
            //}
            if (!Regex.IsMatch(uri.AbsolutePath, @"^/[\w-]+$"))
            {
                uRLStructure.isAbsolutePathValid = "Invalid";
                // Invalid path
            }

            // Additional validation for query parameters if needed

            //string query = uri.Query;
            // Parse and validate query parameters
            NameValueCollection queryParameters = HttpUtility.ParseQueryString(uri.Query);
            List<string[]> queryParams = new List<string[]>();
            if (queryParameters != null)
            {
                for (int i = 0; i < queryParameters.Count; i++)
                {
                    queryParams.Add(queryParameters.GetValues(i));
                }
            }

            return JsonConvert.SerializeObject(uRLStructure);
        }
    }

    public class Img
    {
        public string FileName { get; set; }
        public string AltTag { get; set; }
    }
    public class URLStructure
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public string AbsolutePath { get; set; }
        public string Query { get; set; }
        public string isSchemeValid { get; set; } = "Valid";
        public string isHostValid { get; set; } = "Valid";
        public string isAbsolutePathValid { get; set; } = "Valid";
    }
}
