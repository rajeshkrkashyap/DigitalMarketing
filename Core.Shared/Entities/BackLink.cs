using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Shared.Entities
{
    /// <summary>
    /// The properties and attributes of backlinks (also known as inbound links or incoming links) to a website can vary, and they play a significant role in search engine optimization (SEO). Here are some key properties and attributes associated with backlinks:
    /// Anchor Text:
    /// Property: The clickable text in a hyperlink.
    /// Importance: Relevant and descriptive anchor text can provide context to search engines about the content of the linked page.
    /// Link Source:
    /// Property: The website or webpage that contains the backlink.
    /// Importance: High-quality and reputable sources are generally more valuable for SEO.
    /// Link Destination:
    /// Property: The webpage on the target site that the backlink points to.
    /// Importance: The relevance of the linked page to the content of the source page is crucial for SEO.
    /// DoFollow vs. NoFollow:
    /// Attribute: Links can be either "dofollow" or "nofollow."
    /// Importance: Dofollow links pass SEO value, while nofollow links signal to search engines not to follow the link for ranking purposes.
    /// Link Juice:
    /// Property: The SEO value or authority passed from one page to another through a link.
    /// Importance: Backlinks from authoritative pages can contribute positively to the ranking of the linked page.
    /// Link Position:
    /// Property: The location of the backlink on the source page (e.g., within content, footer, sidebar).
    /// Importance: Links within the main content are often considered more valuable than those in less prominent positions.
    /// Contextual Relevance:
    /// Property: How well the content surrounding the link relates to the linked page's content.
    /// Importance: Contextual relevance enhances the perceived value of the link.
    /// Link Diversity:
    /// Property: The variety of sources linking to a website.
    /// Importance: A diverse backlink profile, including links from various domains, can be more beneficial for SEO.
    /// Link Velocity:
    /// Property: The rate at which a website acquires new backlinks over time.
    /// Importance: Sudden spikes or drops in link acquisition can influence search engine algorithms.
    /// Natural vs.Artificial:
    /// Property: Whether the backlink is naturally earned or obtained through manipulative means.
    /// Importance: Search engines prefer natural, organic link building over manipulative practices.
    /// It's important to note that the quality and relevance of backlinks are often more critical than the sheer quantity. Building a healthy backlink profile involves focusing on high-quality, relevant, and authoritative links. Additionally, search engine algorithms and ranking factors may evolve over time, influencing the impact of different backlink attributes.
    /// </summary>
    public class BackLink
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ProjectId { get; set; }
        public string? AnchorText { get; set; }
        public string? LinkSource { get; set; }
        public string? LinkDestination { get; set; }
        public string? DoFollowOrNoFollow { get; set; }
        public string? LinkJuice { get; set; }
        public string? LinkPosition { get; set; }
        public string? ContextualRelevance { get; set; }
        public string? LinkDiversity { get; set; }
        public string? LinkVelocity { get; set; }
        public string? NaturalOrArtificial { get; set; }
        public string AnalysisStatus { get; set; } = "Pending"; //InProgress/Failed/Completed
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Project? Project { get; set; }
    }
}
