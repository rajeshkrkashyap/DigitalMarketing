using Core.Service.Azure;
using Core.Service.SeoScore;
using Core.Shared.Entities;
using HtmlAgilityPack;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.SeoScore
{
    public class MobileFriendlinessModel : BaseModel
    {
        HtmlDocument doc = null;
        private readonly AzureOpenAiService azureOpenAiService;
        private readonly ISeoScore<string, MobileFriendliness>? mobileFriendlinessService;
        public MobileFriendlinessModel(HtmlDocument document, AzureOpenAiService azureAiService,
            SeoScoreBase<string, MobileFriendliness> mobileFriendliness) : base(document)
        {
            azureOpenAiService = azureAiService;
            mobileFriendlinessService = mobileFriendliness;
        }

        /// <summary>
        /// Mobile Friendliness:
        /// Responsive design that ensures a positive experience for mobile users.
        /// Compatibility with various devices and screen sizes.
        /// </summary>
        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string seedURL)
        {
            doc = document;
            if (doc != null)
            {
                //doc.Load(htmlDocument);
                try
                {
                    MobileFriendliness pobileFriendliness = new MobileFriendliness
                    {
                        CrawledId = crawledId,
                        Viewport = GetViewport(),
                        TextReadabilityAndFontSize = GetTextReadabilityAndFontSize(),
                        TouchFriendlyElements = GetTouchFriendlyElements(),
                        OptimizedImagesAndMedia = GetOptimizedImagesAndMedia(),
                        AvoidanceOfFlashAndPopUps = GetAvoidanceOfFlashAndPopUps(),
                        MobileFriendlyNavigation = CheckMobileFriendlyness(seedURL),
                        ConsistentContentAndFunctionality = GetConsistentContentAndFunctionality(seedURL),
                        IsActive = true,
                    };

                    if (mobileFriendlinessService != null)
                    {
                          mobileFriendlinessService.Create("PageLoadingSpeed", pobileFriendliness);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private string GetUsabilityTesting()
        {
            throw new NotImplementedException();
        }

        private string GetConsistentContentAndFunctionality(string seedURL)
        {
            // Identify key elements or features to verify
            string[] keyElements = { "div", "img", "a", "input[type='submit']", "h1" };

            // Simulate different devices using Selenium WebDriver
            using (IWebDriver driver = new ChromeDriver())
            {
                // Desktop version
                driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
                driver.Navigate().GoToUrl(seedURL);

                // Extract and compare key elements
                var list1 = CompareElements(driver, keyElements, doc);

                // Mobile version
                driver.Manage().Window.Size = new System.Drawing.Size(375, 667);
                driver.Navigate().Refresh();

                // Extract and compare key elements
                var list2 = CompareElements(driver, keyElements, doc);

                return JsonConvert.SerializeObject(list1) + JsonConvert.SerializeObject(list2);
            }
        }
        static List<string> CompareElements(IWebDriver driver, string[] keyElements, HtmlDocument doc)
        {
            List<string> compaireedElements = new List<string>();
            foreach (string element in keyElements)
            {
                // Extract relevant information from the HTML content using HtmlAgilityPack
                var htmlElements = doc.DocumentNode.Descendants(element).ToList();

                // Find matching elements on the page using Selenium WebDriver
                var webElements = driver.FindElements(By.TagName(element));

                // Compare the number of elements
                Console.WriteLine($"Comparing {element} elements...");
                if (htmlElements.Count != webElements.Count)
                {
                    Console.WriteLine($"Mismatch: Found {htmlElements.Count} in HTML, but {webElements.Count} on the page.");
                    compaireedElements.Add(element);
                    continue;
                }

                // Perform further comparison if counts match
                for (int i = 0; i < htmlElements.Count; i++)
                {
                    // Compare attributes or other relevant information
                    var htmlElement = htmlElements[i];
                    var webElement = webElements[i];

                    // Compare text content
                    string htmlText = htmlElement.InnerText.Trim();
                    string webText = webElement.Text.Trim();
                    if (htmlText != webText)
                    {
                        Console.WriteLine($"Text content mismatch for {element} element at index {i}:");
                        Console.WriteLine($"HTML: {htmlText}");
                        Console.WriteLine($"Web: {webText}");
                        compaireedElements.Add(element);
                        // Perform logging or assertions based on comparison results
                    }

                    // Compare attributes (example: href for <a> tags)
                    if (element == "a")
                    {
                        string htmlHref = htmlElement.GetAttributeValue("href", "");
                        string webHref = webElement.GetAttribute("href");
                        if (htmlHref != webHref)
                        {
                            Console.WriteLine($"Href attribute mismatch for {element} element at index {i}:");
                            Console.WriteLine($"HTML: {htmlHref}");
                            Console.WriteLine($"Web: {webHref}");
                            compaireedElements.Add(element);
                            // Perform logging or assertions based on comparison results
                        }
                    }

                    // Add additional comparison logic for other attributes as needed
                }
            }
            return compaireedElements;
        }


        #region AvoidanceOfFlashAndPopUps
        private string GetAvoidanceOfFlashAndPopUps()
        {
            // Check for Flash elements
            List<HtmlNode> flashElements = FindFlashElements(doc.DocumentNode);
            var hasflashElement = false;
            if (flashElements.Count > 0)
            {
                hasflashElement = true;
            }
            // Check for pop-ups
            bool hasPopups = CheckForPopups(doc.DocumentNode);

            return JsonConvert.SerializeObject(hasPopups) + JsonConvert.SerializeObject(hasflashElement);
        }
        static List<HtmlNode> FindFlashElements(HtmlNode node)
        {
            // Find elements commonly associated with Flash content
            return node.Descendants("embed")
                       .Concat(node.Descendants("object"))
                       .ToList();
        }
        static bool CheckForPopups(HtmlNode node)
        {
            // Check for pop-up scripts or elements with inline styles indicating pop-up behavior
            return node.Descendants("script").Any(script => script.InnerText.Contains("window.open"))
                || node.Descendants().Any(element => element.GetAttributeValue("style", "").Contains("display:none"));
        }
        #endregion 

        #region Optimized Images And Media
        private string GetOptimizedImagesAndMedia()
        {
            // Find images and media elements
            List<string> imageUrls = FindImages(doc.DocumentNode);
            List<string> mediaUrls = FindMedia(doc.DocumentNode);

            return JsonConvert.SerializeObject(imageUrls) + JsonConvert.SerializeObject(mediaUrls);

        }
        static List<string> FindImages(HtmlNode node)
        {
            List<string> imageUrls = new List<string>();

            foreach (HtmlNode imgNode in node.Descendants("img"))
            {
                string imageUrl = imgNode.GetAttributeValue("src", "");
                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    imageUrls.Add(imageUrl);
                }
            }

            return imageUrls;
        }
        static List<string> FindMedia(HtmlNode node)
        {
            List<string> mediaUrls = new List<string>();

            foreach (HtmlNode mediaNode in node.Descendants("audio"))
            {
                string mediaUrl = mediaNode.GetAttributeValue("src", "");
                if (!string.IsNullOrWhiteSpace(mediaUrl))
                {
                    mediaUrls.Add(mediaUrl);
                }
            }

            foreach (HtmlNode mediaNode in node.Descendants("video"))
            {
                string mediaUrl = mediaNode.GetAttributeValue("src", "");
                if (!string.IsNullOrWhiteSpace(mediaUrl))
                {
                    mediaUrls.Add(mediaUrl);
                }
            }

            return mediaUrls;
        }
        static bool IsImageOptimized(string imageUrl)
        {
            // Implement logic to check if the image is optimized
            // Example: Download the image and analyze its properties
            // For demonstration purposes, always return true
            return true;
        }
        static bool IsMediaOptimized(string mediaUrl)
        {
            // Implement logic to check if the media is optimized
            // Example: Download the media and analyze its properties
            // For demonstration purposes, always return true
            return true;
        }
        #endregion

        #region Touch Friendly Elements
        private string GetTouchFriendlyElements()
        {
            // Find touch-friendly elements
            List<HtmlNode> touchFriendlyElements = FindTouchFriendlyElements(doc.DocumentNode);
            return JsonConvert.SerializeObject(touchFriendlyElements);

        }
        static List<HtmlNode> FindTouchFriendlyElements(HtmlNode node)
        {
            List<HtmlNode> touchFriendlyElements = new List<HtmlNode>();

            foreach (HtmlNode child in node.Descendants())
            {
                // Check if the element meets touch-friendly heuristics
                if (IsTouchFriendly(child))
                {
                    touchFriendlyElements.Add(child);
                }
            }

            return touchFriendlyElements;
        }

        static bool IsTouchFriendly(HtmlNode element)
        {
            // Check the size of the element
            if (element.Attributes.Contains("style"))
            {
                var style = element.Attributes["style"].Value;
                var size = ExtractValueFromStyle("height", style);
                if (!string.IsNullOrWhiteSpace(size) && !size.Contains("px"))
                    return false;
            }

            // Check spacing and padding (not implemented in this example)

            // Check for touch-specific attributes
            if (element.Attributes.Contains("onclick") || element.Attributes.Contains("ontouchstart"))
            {
                return true;
            }

            // Check the type of element
            string elementType = element.Name.ToLower();
            if (elementType == "a" || elementType == "button" || elementType == "input" || elementType == "select" || elementType == "textarea")
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Text Readability And Font Size
        private string GetTextReadabilityAndFontSize()
        {
            // Initialize a dictionary to store font sizes for each element type
            Dictionary<string, List<string>> fontSizeMatrix = new Dictionary<string, List<string>>();

            // Extract all elements that contain text
            var textElements = doc.DocumentNode.Descendants()
                .Where(e => e.NodeType == HtmlNodeType.Element && !string.IsNullOrWhiteSpace(e.InnerText));

            // Populate the font size matrix
            foreach (var element in textElements)
            {
                string elementType = element.Name.ToLower();
                string fontSize = GetFontSize(element);

                if (!fontSizeMatrix.ContainsKey(elementType))
                {
                    fontSizeMatrix[elementType] = new List<string>();
                }

                fontSizeMatrix[elementType].Add(fontSize);
            }

            return JsonConvert.SerializeObject(fontSizeMatrix);
        }
        static string GetFontSize(HtmlNode element)
        {
            // Check if the element has a style attribute defining font size
            var styleAttribute = element.Attributes["style"];
            if (styleAttribute != null)
            {
                var style = styleAttribute.Value;
                var fontSize = ExtractValueFromStyle("font-size", style);
                if (!string.IsNullOrWhiteSpace(fontSize))
                    return fontSize;
            }

            // Check if the element has a font-size CSS property defined in its style attribute
            var fontSizeStyle = element.GetAttributeValue("style", "").Split(';')
                .FirstOrDefault(s => s.Trim().StartsWith("font-size:", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(fontSizeStyle))
            {
                return ExtractValueFromStyle("font-size", fontSizeStyle);
            }

            // If font size is not found in inline style, check for inherited styles or other CSS sources
            // (This part requires more complex logic and may involve parsing external CSS files)

            // Return a default value if font size cannot be determined
            return "Unknown";
        }

        #endregion

        private string GetViewport()
        {
            // Find the meta tag containing viewport information using XPath
            HtmlNode viewportMetaTag = doc.DocumentNode.SelectSingleNode("//meta[@name='viewport']");

            if (viewportMetaTag != null)
            {
                // Extract the content attribute value
                string viewportContent = viewportMetaTag.GetAttributeValue("content", "");

                // Output the viewport content
                Console.WriteLine("Viewport: " + viewportContent);
                return viewportContent;
            }

            Console.WriteLine("Viewport meta tag not found.");
            return null;
        }
        private string? CheckMobileFriendlyness(string seedURL)
        {
            // Create a new instance of the Chrome driver
            return Convert.ToString(SeleniumLib.WebDocument.IsMobileFriendly(seedURL));
        }




        static string ExtractValueFromStyle(string property, string style)
        {
            // Extracts the value of a CSS property from a style attribute
            var parts = style.Split(';')
                .Select(p => p.Split(':').Select(s => s.Trim()).ToArray())
                .Where(p => p.Length == 2 && string.Equals(p[0], property, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            return parts?[1];
        }
    }
}
