using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    /// <summary>
    ///Meta Tags:
    ///Optimized title tags that accurately describe the content of the page.
    ///Compelling and informative meta descriptions that encourage clicks from search engine results.
    /// </summary>
    public class MetaTag
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CrawledId { get; set; }

        /// <summary>
        /// Title Tag:
        /// Use: Specifies the title of the webpage, which appears as the clickable headline in search engine results.
        /// Example: <title>Page Title</title>
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Meta Description:
        /// Use: Provides a brief summary of the webpage's content.
        /// Example: <meta name = "description" content="This is a brief summary of the webpage content.">
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Meta Keywords:
        /// Use: Provides list of Keywords of the webpage's content.
        /// Example: <meta name = "Keywords" content=" Connect to Ai, Ai">
        /// </summary>
        public string? Keywords { get; set; }

        /// <summary>
        /// Viewport Meta Tag:
        /// Use: Specifies the viewport properties for responsive design, ensuring proper rendering and scaling on different devices.
        /// Example: <meta name = "viewport" content="width=device-width, initial-scale=1.0">
        /// </summary>
        public string? Viewport { get; set; }

        /// <summary>
        /// Charset Meta Tag:
        /// Use: Specifies the character encoding for the webpage.
        /// Example: <meta charset = "UTF-8" >
        /// </summary>
        public string? Charset { get; set; }
        /// <summary>
        /// Robots Meta Tag:
        /// Use: Controls how search engine crawlers index and display the webpage.
        /// Example: <meta name = "robots" content= "index, follow" >
        /// </summary>
        public string? Robots { get; set; }

        /// <summary>
        /// Canonical Tag:
        /// Use: Specifies the preferred URL for a webpage, particularly useful for preventing duplicate content issues.
        /// Example: <link rel = "canonical" href= "https://www.example.com/page" >
        /// </summary>
        public string? Canonical { get; set; }

        /// <summary>
        /// Open Graph title will store and retrive value  
        /// </summary>
        public string? OgTitle { get; set; }
        public string? OgDescription { get; set; }
        public string? OgImage { get; set; }
        public string? OgType { get; set; }
        public string? OgUrl { get; set; }

        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Crawled? Crawled { get; set; }
    }
}
