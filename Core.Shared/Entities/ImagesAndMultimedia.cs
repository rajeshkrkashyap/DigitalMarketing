using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    /// <summary>
    /// Enhanced User Experience: Visual elements such as images, videos, infographics, and 
    /// interactive media enhance the overall user experience by making the content more engaging 
    /// and visually appealing. Users are more likely to stay on a webpage longer and interact 
    /// with the content when it includes multimedia elements.
    /// 
    /// Illustration and Explanation: Images and multimedia can be used to illustrate concepts, 
    /// provide examples, or explain complex ideas more effectively than text alone. Visual aids 
    /// help to clarify information and make it easier for users to understand.
    /// 
    /// Increased Engagement: Multimedia content tends to attract more attention and engagement 
    /// from users compared to text-only content. Including compelling images, videos, or 
    /// interactive elements encourages users to interact with the content, share it on social 
    /// media, and contribute to a higher level of user engagement.
    /// 
    /// Improved Accessibility: Multimedia content can enhance accessibility for users with 
    /// disabilities, such as those who are visually impaired. Providing alternative text 
    /// descriptions (alt text) for images and captions for videos makes the content more 
    /// accessible to a wider audience and improves usability.
    /// 
    /// Reduced Bounce Rate: Engaging multimedia content can help reduce the bounce rate of a 
    /// webpage, which is the percentage of visitors who navigate away from the site after viewing
    /// only one page. By capturing users' attention and encouraging them to explore further, 
    /// multimedia content can lower the bounce rate and improve user retention.
    /// 
    /// SEO Benefits: Including optimized images and multimedia can also have positive effects on 
    /// the webpage's SEO (Search Engine Optimization) score. When properly optimized with 
    /// descriptive file names, alt text, and captions, multimedia elements can improve the 
    /// relevance and context of the content for search engines, leading to higher rankings in 
    /// search results.
    /// 
    /// Rich Snippets in Search Results: Search engines may display rich snippets in search results 
    /// for webpages that include multimedia content such as videos or images. Rich snippets provide
    /// additional visual information, such as thumbnail images or video previews, which can attract
    /// more clicks and improve the visibility of the webpage in search results.
    /// 
    /// Backlink Opportunities: Compelling multimedia content has a higher potential for earning
    /// backlinks from other websites and social media platforms. When users find multimedia 
    /// content valuable or share-worthy, they are more likely to link back to it from their own 
    /// websites or social media profiles, which can improve the webpage's authority and SEO 
    /// performance.
    /// </summary>
    public class ImagesAndMultimedia
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CrawledId { get; set; }     
        public string? FileName { get; set; }
        public string? AltText { get; set; }
        public string? SizeAndFormat{ get; set; }
        public string? MediaSitemap { get; set; }
        public string? PageContext { get; set; }
        public string? ResponsiveDesign { get; set; }
        public string? Type { get; set; }
        public string? MultimediaURL { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Crawled? Crawled { get; set; }
    }
}
