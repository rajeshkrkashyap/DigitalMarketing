using Core.Service.Azure;
using Core.Service.SeoScore;
using Core.Shared.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ServerLib.SeoScore
{
    public class ImagesAndMultimediaModel : BaseModel
    {
        HtmlDocument doc = null;
        private readonly AzureOpenAiService azureOpenAiService;
        private readonly ISeoScore<string, ImagesAndMultimedia>? imagesAndMultimediaService;
        public ImagesAndMultimediaModel(HtmlDocument document, AzureOpenAiService azureAiService,
            SeoScoreBase<string, ImagesAndMultimedia> imagesAndMultimedia) : base(document)
        {
            azureOpenAiService = azureAiService;
            imagesAndMultimediaService = imagesAndMultimedia;
        }

        /// <summary>
        /// Images and Multimedia:
        /// Optimized images with descriptive alt text.
        /// Proper use of multimedia elements to enhance user engagement.
        /// 
        /// 1. File Naming: Use regular expressions to validate filenames for adherence to naming conventions and keyword inclusion.
        /// 2. Alt Text: Write functions to check the presence and quality of alt text for each image. You can use HTML parsing 
        ///    libraries like HtmlAgilityPack to extract and analyze HTML content.
        /// 3. Image Size and Format: Utilize image processing libraries like ImageMagick or System.Drawing in C# to check image sizes and
        ///    formats. Ensure that they meet the recommended guidelines for web optimization.
        /// 4. Image Sitemap: Generate and validate image sitemaps programmatically using C# to ensure they adhere to the XML sitemap protocol.
        /// 5. Page Context: Analyze the content of web pages programmatically to ensure that images and multimedia files are contextually relevant.
        ///    You can use web scraping techniques with libraries like HtmlAgilityPack to parse and analyze page content.
        /// 6. Responsive Design: Test the responsiveness of images and multimedia files across different devices using C# 
        ///    testing frameworks like Selenium or specialized libraries for web scraping and testing.
        /// </summary>

        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string _seedUrl)
        {
            doc = document;
            if (doc != null)
            {
               // doc.Load(htmlDocument);

                var imageNodes = doc.DocumentNode.SelectNodes("//img");
                foreach (var imageNode in imageNodes)
                {
                    ImagesAndMultimedia imagesAndMultimedia = new ImagesAndMultimedia
                    {
                        CrawledId = crawledId,
                        FileName = GetFileName(imageNode),
                        AltText = GetAltText(imageNode),
                        SizeAndFormat = GetSizeAndFormat(imageNode),
                        MediaSitemap = GetMediaSitemap(imageNode),
                        PageContext = GetPageContext(imageNode),
                        ResponsiveDesign = GetResponsiveDesign(imageNode),
                        Type = GetType(imageNode),
                        MultimediaURL = GetMultimediaURL(imageNode),
                        IsActive = true
                    };
                    try
                    {
                        imagesAndMultimediaService.Create("ImagesAndMultimedia", imagesAndMultimedia);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        // Example methods to extract information about each image
        string GetFileName(HtmlNode imageNode)
        {
            // Extract the value of the "src" attribute from the imageNode
            string srcAttributeValue = imageNode.GetAttributeValue("src", "");

            // If the "src" attribute contains a valid file path, extract the file name
            if (!string.IsNullOrWhiteSpace(srcAttributeValue))
            {
                // Split the "src" attribute value by '/' to extract the file name
                string[] pathSegments = srcAttributeValue.Split('/');

                // Get the last segment of the path, which typically represents the file name
                string fileName = pathSegments.LastOrDefault();

                // You may need to further process the file name to handle cases where the image source might contain query parameters or fragments
                // For example, you can split the file name by '?' or '#' and take the first part as the file name

                return fileName;
            }
            else
            {
                // If the "src" attribute is empty or null, return an empty string or handle the case as per your requirement
                return "";
            }
        }
        string GetAltText(HtmlNode imageNode)
        {
            // Implement logic to extract the alt text from the image
            return imageNode.GetAttributeValue("alt", "");
        }
        string GetSizeAndFormat(HtmlNode imageNode)
        {
            // Extract width and height attributes of the image
            string width = imageNode.GetAttributeValue("width", "");
            string height = imageNode.GetAttributeValue("height", "");

            // If both width and height attributes are present, return the size in the format "width x height"
            if (!string.IsNullOrWhiteSpace(width) && !string.IsNullOrWhiteSpace(height))
            {
                return $"{width}x{height}";
            }
            else
            {
                // If either width or height attribute is missing, return an empty string or handle the case as per your requirement
                return "";
            }
        }
        string GetMediaSitemap(HtmlNode imageNode)
        {
            // Implement logic to determine if the image should be included in the media sitemap
            // Check if the image is decorative (i.e., it doesn't convey meaningful information)
            // Implement logic to determine if the image is decorative (e.g., if it's used for visual styling only)
            // For example, check if the image has an empty or missing alt attribute, which is often indicative of a decorative image
            string altText = imageNode.GetAttributeValue("alt", "").Trim();

            // If the alt attribute is empty or missing, consider the image decorative
            bool isDecorative = string.IsNullOrEmpty(altText);

            // Decide whether to include the image in the media sitemap based on its purpose
            if (isDecorative)
            {
                // If the image is decorative, it may not need to be included in the media sitemap
                return "Exclude"; // You can customize this return value based on your sitemap generation logic
            }
            else
            {
                // If the image conveys meaningful information, include it in the media sitemap
                return "Include"; // You can customize this return value based on your sitemap generation logic
            }
        }
        string GetPageContext(HtmlNode imageNode)
        {
            // Implement logic to extract contextual information about the image
            // Initialize an empty string to store the page context information
            string pageContext = "";

            // Traverse up the DOM tree to find the nearest parent node that provides context to the image
            HtmlNode parentNode = imageNode.ParentNode;
            while (parentNode != null)
            {
                // Check if the parent node contains relevant contextual information (e.g., <div>, <p>, <figure>)
                if (parentNode.Name.Equals("div", StringComparison.OrdinalIgnoreCase) ||
                    parentNode.Name.Equals("p", StringComparison.OrdinalIgnoreCase) ||
                    parentNode.Name.Equals("figure", StringComparison.OrdinalIgnoreCase))
                {
                    // Extract text content from the parent node to include in the page context
                    string parentText = parentNode.InnerText.Trim();
                    if (!string.IsNullOrEmpty(parentText))
                    {
                        // Append the parent text to the page context string
                        pageContext += parentText + " ";
                    }
                }

                // Move up to the next parent node
                parentNode = parentNode.ParentNode;
            }

            // Trim any leading or trailing whitespace from the page context string
            pageContext = pageContext.Trim();

            // Return the extracted page context
            return pageContext;
        }
        string GetResponsiveDesign(HtmlNode imageNode)
        {
            // Implement logic to determine if the image is responsive
            // Check if the image has the "class" attribute containing any classes related to responsive design
            string classAttributeValue = imageNode.GetAttributeValue("class", "");

            // Define classes that are typically used for responsive images
            string[] responsiveClasses = { "img-responsive", "img-fluid", "responsive-img", "responsive-image" };

            // Check if any of the responsive classes are present in the "class" attribute
            foreach (string responsiveClass in responsiveClasses)
            {
                if (classAttributeValue.Contains(responsiveClass, StringComparison.OrdinalIgnoreCase))
                {
                    // If any of the responsive classes are found, return "Responsive"
                    return "Responsive";
                }
            }

            // If none of the responsive classes are found, return "Not Responsive"
            return "Not Responsive";
        }
        string GetType(HtmlNode imageNode)
        {
            // Implement logic to determine the type of image (e.g., photo, illustration)
            // Extract the value of the "src" attribute from the imageNode
            string srcAttributeValue = imageNode.GetAttributeValue("src", "").ToLower();

            // Define keywords to identify different types of images
            string[] photoKeywords = { "photo", "picture", "image", "photograph" };
            string[] illustrationKeywords = { "illustration", "drawing", "art", "sketch" };

            // Check if the "src" attribute contains any keywords indicative of photo or illustration
            foreach (string keyword in photoKeywords)
            {
                if (srcAttributeValue.Contains(keyword))
                {
                    // If any photo keyword is found in the "src" attribute, return "Photo"
                    return "Photo";
                }
            }

            foreach (string keyword in illustrationKeywords)
            {
                if (srcAttributeValue.Contains(keyword))
                {
                    // If any illustration keyword is found in the "src" attribute, return "Illustration"
                    return "Illustration";
                }
            }

            // If no specific type can be determined, return "Unknown"
            return "Unknown";
        }
        string GetMultimediaURL(HtmlNode imageNode)
        {
            // Implement logic to extract the URL of the multimedia content (if applicable)
            // Extract the value of the "src" attribute from the imageNode
            string srcAttributeValue = imageNode.GetAttributeValue("src", "");

            // Return the value of the "src" attribute as the multimedia URL
            return srcAttributeValue;
        }

        private static bool ValidateFilename(string filename)
        {
            // Define the regular expression pattern for file naming conventions
            //This regular expression now covers file extensions for JPEG(.jpg, .jpeg), PNG(.png), GIF(.gif), BMP(.bmp), and SVG(.svg) image formats.
            var pattern = "^[a-zA-Z0-9-]+\\.((jpg)|(png)|(gif)|(jpeg)|(bmp)|(svg)|(mp4)|(avi)|(mov)|(wmv)|(flv)|(mkv)|(mp3)|(wav)|(ogg)|(aac))$";
            // Use regular expression to match filename against pattern
            Regex regex = new Regex(pattern);
            return regex.IsMatch(filename);
        }
        static bool IsAltTextQualityAcceptable(string altText)
        {
            // Example criteria for alt text quality (you can customize this based on your requirements)
            const int MinLength = 5; // Minimum length of alt text
            const int MaxLength = 100; // Maximum length of alt text

            // Check length
            if (altText.Length < MinLength || altText.Length > MaxLength)
            {
                return false;
            }

            // Additional criteria can be added here (e.g., relevance of keywords)
            return true;
        }
    }
}
