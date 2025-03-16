using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    /// <summary>
    /// Measuring the content quality of a webpage involves assessing various factors that contribute to the 
    /// usefulness, relevance, credibility, and engagement level of the content. While there's no single 
    /// definitive metric for content quality, here are several key aspects to consider when evaluating the 
    /// quality of a webpage's content:
    /// </summary>
    public class ContentQuality
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CrawledId { get; set; }
        public string? PageTitle { get; set; }

        /// <summary>
        /// Relevance to the Topic: Determine whether the content aligns with the main topic or purpose
        /// of the webpage. Irrelevant or off-topic content can detract from the overall quality. 
        /// </summary>
        public bool IsRelevanceToTheTopic { get; set; }
        /// <summary>
        /// Accuracy and Credibility: Evaluate the accuracy and reliability of the information
        /// presented. Look for authoritative sources, citations, and evidence to support claims.
        /// Content from reputable sources tends to be more trustworthy. 
        /// </summary>
        public bool IsAccuracyAndCredibility { get; set; }
        /// <summary>
        /// Clarity and Readability: Assess the clarity and readability of the content. Is the language
        /// clear and concise? Are complex concepts explained in a way that's easy for the target
        /// audience to understand? Proper grammar, spelling, and formatting contribute to readability. 
        /// </summary>
        public bool IsClarityAndReadability { get; set; }
        /// <summary>
        /// Depth and Breadth of Coverage: Consider the depth and breadth of coverage on the topic.
        /// High-quality content provides comprehensive information that addresses key aspects of the subject 
        /// matter. It may include examples, case studies, or multimedia elements to enhance understanding. 
        /// </summary>
        public bool IsDepthAndBreadthOfCoverage { get; set; }
        /// <summary>
        /// Uniqueness and Originality: Determine whether the content offers a unique perspective or adds 
        /// value beyond what's available elsewhere. Original content that offers fresh insights or presents 
        /// information in a novel way is more likely to be considered high quality. 
        /// </summary>
        public string? UniquenessAndOriginality { get; set; }
        /// <summary>
        /// Engagement and Interactivity: Evaluate how engaging the content is for the audience. 
        /// Does it encourage interaction, such as through comments, social sharing, or interactive 
        /// elements? Engaging content tends to keep users on the page longer and encourages them to 
        /// take action. 
        /// </summary>
        public string? EngagementAndInteractivity { get; set; }
        /// <summary>
        /// User Experience (UX): Consider the overall user experience of the webpage. Is the layout 
        /// intuitive and easy to navigate? Does the content load quickly and display properly on 
        /// various devices and screen sizes? A positive UX enhances the perceived quality of the content. 
        /// </summary>
        public string? UserExperience { get; set; }
        /// <summary>
        /// Reputation and Authority: Assess the reputation and authority of the website or author publishing 
        /// the content. Established websites with a history of producing high-quality content are more likely 
        /// to maintain consistent standards. 
        /// </summary>
        public bool IsReputationAndAuthority { get; set; }
        /// <summary>
        /// Purpose and Intent: Consider the intended purpose of the content. Is it educational, informative, 
        /// entertaining, or promotional? High-quality content effectively fulfills its intended purpose 
        /// and meets the needs of the target audience. 
        /// </summary>
        public bool IsPurposeAndIntent { get; set; }
        /// <summary>
        /// Feedback and Metrics: Gather feedback from users, such as through comments, ratings, or surveys. 
        /// Analyze metrics such as time on page, bounce rate, and social shares to gauge user engagement 
        /// and satisfaction with the content. 
        /// </summary>
        public bool IsFeedbackAndMetrics { get; set; }

        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public virtual Crawled? Crawled { get; set; }

    }
}
