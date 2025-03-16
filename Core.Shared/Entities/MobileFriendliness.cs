using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Shared.Entities
{
    /// <summary>
    /// Mobile Friendliness:
    /// Responsive design that ensures a positive experience for mobile users.
    /// Compatibility with various devices and screen sizes.
    /// </summary>
    public class MobileFriendliness
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CrawledId { get; set; }

        /// <summary>
        ///Viewport Meta Tag: Check if the viewport meta tag is present in the HTML head section.
        ///This tag ensures proper scaling and rendering on mobile devices.Verify that it's configured 
        ///correctly to support various screen sizes.
        /// </summary>
        public string? Viewport { get; set; }

        /// <summary>
        /// Text Readability and Font Size: Assess the readability of text on the page. Ensure that font 
        /// sizes are sufficiently large and legible on smaller screens without the need for zooming. 
        /// Text should flow smoothly without horizontal scrolling.
        /// </summary>
        public string? TextReadabilityAndFontSize { get; set; }

        /// <summary>
        /// Touch-Friendly Elements: Evaluate the size and spacing of clickable elements like buttons, links, 
        /// and form inputs. They should be large enough to be easily tapped with a finger on touchscreen devices,
        /// with enough space between them to avoid accidental taps.
        /// </summary>
        public string? TouchFriendlyElements { get; set; }

        /// <summary>
        /// Optimized Images and Media: Check if images and multimedia content are optimized for mobile devices. 
        /// They should load quickly and be appropriately sized for smaller screens to reduce bandwidth usage and 
        /// improve loading times.
        /// </summary>
        public string? OptimizedImagesAndMedia { get; set; }

 
        /// <summary>
        /// Avoidance of Flash and Pop-ups: Ensure that the page doesn't rely on Flash or contain intrusive pop-ups, 
        /// as these elements are not mobile-friendly and can negatively impact user experience.
        /// </summary>
        public string? AvoidanceOfFlashAndPopUps { get; set; }

        /// <summary>
        /// Mobile-Friendly Navigation: Evaluate the navigation menu and site structure for ease of use on mobile 
        /// devices. Consider using mobile-friendly navigation patterns such as collapsible menus or hamburger 
        /// icons to conserve screen space.
        /// </summary>
        public string? MobileFriendlyNavigation { get; set; }

        /// <summary>
        /// Consistent Content and Functionality: Verify that the content and functionality of the page remain 
        /// consistent across different devices. Users should be able to access the same information and features 
        /// regardless of the device they're using.
        /// </summary>
        public string? ConsistentContentAndFunctionality { get; set; }

 

        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Crawled? Crawled { get; set; }
    }
}
