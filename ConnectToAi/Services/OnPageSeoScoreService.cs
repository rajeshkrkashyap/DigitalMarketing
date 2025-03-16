using Core.Shared;
using Core.Shared.Entities;
using HtmlAgilityPack;

namespace ConnectToAi.Services
{
    /// <summary>
    /// The on-page SEO score of a website is influenced by various factors that search 
    /// engines consider when evaluating the relevance and quality of a web page. 
    /// While different tools and platforms may use slightly different criteria, common 
    /// parameters to calculate on-page SEO score typically include:
    /// </summary>
    public class OnPageSeoScoreService : BaseService
    {
        private readonly HtmlDocument doc = null;
        public OnPageSeoScoreService(HtmlDocument document, ConfigService configService) : base(configService)
        {
            doc = document;
        }

        /// <summary>
        /// Keyword Usage:
        /// Presence of target keywords in the title tag, meta description, heading tags (H1, H2, etc.), 
        /// and throughout the content.
        /// Keyword density, ensuring a natural and appropriate distribution of keywords within the content.
        /// </summary>
        public KeywordUsage KeywordUsage { get; set; }

        /// <summary>
        /// Content Quality:
        /// High-quality, relevant, and original content that meets the user's search intent.
        /// Readability and structure of content, including proper use of headings, paragraphs, and bullet points.
        /// </summary>
        public ContentQuality ContentQuality { get; set; }

        /// <summary>
        /// Meta Tags:
        /// Optimized title tags that accurately describe the content of the page.
        /// Compelling and informative meta descriptions that encourage clicks from search engine results.
        /// </summary>
        public MetaTag MetaTags { get; set; }

        /// <summary>
        /// URL Structure:
        /// Clean and descriptive URLs that include relevant keywords. Avoidance of unnecessary parameters, 
        /// symbols, or numbers in URLs.
        /// </summary>
        public URLStructure URLStructure { get; set; }

        /// <summary>
        /// Internal Linking:
        /// Effective use of internal links to connect related pages within the website. Anchor text that 
        /// provides context for the linked content.
        /// </summary>
        public InternalLinking InternalLinking { get; set; }

        /// <summary>
        /// Images and Multimedia:
        /// Optimized images with descriptive alt text.
        /// Proper use of multimedia elements to enhance user engagement.
        /// </summary>
        public ImagesAndMultimedia ImagesAndMultimedia { get; set; }

        /// <summary>
        /// Page Loading Speed:
        /// Fast page loading times for a positive user experience.
        /// Optimization of images, code, and server response times to improve speed.
        /// </summary>
        public PageLoadingSpeed PageLoadingSpeed { get; set; }

        /// <summary>
        /// Mobile Friendliness:
        /// Responsive design that ensures a positive experience for mobile users.
        /// Compatibility with various devices and screen sizes.
        /// </summary>
        public MobileFriendliness MobileFriendliness { get; set; }

        /// <summary>
        /// Social Signals:
        /// Presence of social media sharing options.
        /// Integration of social media meta tags for improved visibility when shared.
        /// </summary>
        public SocialSignal SocialSignals { get; set; }

        /// <summary>
        /// Technical SEO:
        /// Proper use of canonical tags to address duplicate content issues.
        /// XML sitemap availability for search engines.
        /// Correct implementation of redirects (301, 302).
        /// </summary>
        public TechnicalSEO TechnicalSEO { get; set; }

        /// <summary>
        /// Security:
        /// Use of HTTPS to ensure a secure connection, which is also a ranking factor.
        /// </summary>
        public Security Security { get; set; }

        public void LoadDocument(string htmlContent)
        {
            doc.LoadHtml(htmlContent);
        }

        private string ExtractWebPageHeaderContent()
        {
            // Extract header content
            var headerNode = doc.DocumentNode.SelectSingleNode("//header");
            return headerNode != null ? headerNode.InnerHtml : "Header not found"; ;
        }
        private string ExtractWebPageInnerContent()
        {
            //Extract main content body
            var mainNode = doc.DocumentNode.SelectSingleNode("//main");
            return mainNode != null ? mainNode.InnerHtml : "Main content not found";
        }
        private string ExtractWebPagefooterContent()
        {
            // Extract footer content
            var footerNode = doc.DocumentNode.SelectSingleNode("//footer");
            return footerNode != null ? footerNode.InnerHtml : "Footer not found";
        }
    }
}
