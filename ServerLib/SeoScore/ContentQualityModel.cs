using Core.Shared.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service.SeoScore;
using Core.Shared;
using System.Text.RegularExpressions;
using System.Net;
using Newtonsoft.Json;
using Core.Service.Azure;
using System.Reflection.Metadata;
using System.Data;
using System.Xml;

namespace ServerLib.SeoScore
{
    public class ContentQualityModel : BaseModel
    {
        HtmlDocument doc = null;
        private readonly AzureOpenAiService azureOpenAiService;
        private readonly ISeoScore<string, ContentQuality>? contentQualityService;
        private readonly ISeoScore<string, RelevanceToTheTopic>? relevanceToTheTopicService;
        private readonly ISeoScore<string, AccuracyAndCredibility>? accuracyAndCredibilityService;
        private readonly ISeoScore<string, ClarityAndReadability>? clarityAndReadabilityService;
        private readonly ISeoScore<string, DepthAndBreadthOfCoverage>? depthAndBreadthOfCoverageService;
        private readonly ISeoScore<string, ReputationAndAuthority>? reputationAndAuthorityService;
        private readonly ISeoScore<string, PurposeAndIntent>? purposeAndIntentService;

        /// <summary>
        /// Content Quality:
        /// High-quality, relevant, and original content that meets the user's search intent.
        /// Readability and structure of content, including proper use of headings, paragraphs, and bullet points.
        /// </summary>
        public ContentQualityModel(HtmlDocument document, AzureOpenAiService azureAiService,
            SeoScoreBase<string, ContentQuality> contentQuality,
            SeoScoreBase<string, RelevanceToTheTopic> relevanceToTheTopic,
            SeoScoreBase<string, AccuracyAndCredibility>? accuracyAndCredibility,
            SeoScoreBase<string, ClarityAndReadability>? clarityAndReadability,
            SeoScoreBase<string, DepthAndBreadthOfCoverage>? depthAndBreadthOfCoverage,
            SeoScoreBase<string, ReputationAndAuthority>? reputationAndAuthority,
            SeoScoreBase<string, PurposeAndIntent>? purposeAndIntent) : base(document)
        {
            azureOpenAiService = azureAiService;

            contentQualityService = contentQuality;
            relevanceToTheTopicService = relevanceToTheTopic;
            accuracyAndCredibilityService = accuracyAndCredibility;
            clarityAndReadabilityService = clarityAndReadability;
            depthAndBreadthOfCoverageService = depthAndBreadthOfCoverage;
            reputationAndAuthorityService = reputationAndAuthority;
            purposeAndIntentService = purposeAndIntent;
        }

        public override void Process(HtmlDocument document, Project project, List<string> ignoreWordList, string htmlDocument, string crawledId, string _seedUrl)
        {
            doc = document;
            if (doc != null)
            {
                Domain = project.URL;
               // doc.Load(htmlDocument);

                var contentQuality = new ContentQuality
                {
                    CrawledId = crawledId,
                    PageTitle = "",
                    UniquenessAndOriginality = "",
                    EngagementAndInteractivity = "",
                    UserExperience = "",
                    IsRelevanceToTheTopic = false,
                    IsAccuracyAndCredibility = false,
                    IsClarityAndReadability = false,
                    IsDepthAndBreadthOfCoverage = false,
                    IsReputationAndAuthority = false,
                    IsPurposeAndIntent = false,
                    IsFeedbackAndMetrics = false,
                };

                contentQualityService?.Create("ContentQuality", contentQuality);

                var contentQualityUpdate = new ContentQuality
                {
                    CrawledId = contentQuality.CrawledId,
                    PageTitle = contentQuality.PageTitle,
                    UniquenessAndOriginality = GetUniquenessAndOriginality(doc),
                    EngagementAndInteractivity = GetEngagementAndInteractivity(doc),
                    UserExperience =  GetUserExperience(doc).Result,
                    IsRelevanceToTheTopic =  GetRelevanceToTheTopic(doc, contentQuality.Id).Result,
                    IsAccuracyAndCredibility =  GetAccuracyAndCredibility(project, doc, contentQuality.Id).Result,
                    IsClarityAndReadability =  GetClarityAndReadability(doc, contentQuality.Id).Result,
                    IsDepthAndBreadthOfCoverage =  GetDepthAndBreadthOfCoverage(doc, contentQuality.Id).Result,
                    IsReputationAndAuthority =  GetReputationAndAuthority(project, doc, contentQuality.Id).Result,
                    IsPurposeAndIntent =  GetPurposeAndIntent(doc, contentQuality.Id).Result,
                    IsFeedbackAndMetrics = GetFeedbackAndMetrics(doc, contentQuality.Id),
                };

                contentQualityService?.Update("ContentQuality", contentQualityUpdate);
            }
        }

        #region Private Methods
        /// <summary>
        /// Relevance to the Topic: Determine whether the content aligns with the main topic or purpose
        /// of the webpage. Irrelevant or off-topic content can detract from the overall quality. 
        /// </summary>
        private async Task<bool> GetRelevanceToTheTopic(HtmlDocument doc, string contentQualityId)
        {
            HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//body");
            string textContent = bodyNode.InnerHtml;
            if (string.IsNullOrEmpty(textContent))
            {
                var responseFormat = "Your response always in below JSON Format.\n"
                    + " 'RelevanceToTheTopic': {"
                    + "   'understandingOfTopic': null,"
                    + "   'analysisOfContent': null,"
                    + "   'consistencyCheck': null,"
                    + "   'relevanceAssessment': null,"
                    + "   'considerationOfAudienceIntent': null,"
                    + "   'feedbackOfContent': null,"
                    + "   'totalScore': 0"
                    + "}";

                var userMessagePrompt = "Please determine whether content aligns with the main topic or purpose of a webpage title/heading, you can follow these steps"
                   + " Also, calculate the TotalScore based on all the points:\n"
                   + " 1.Understand the Main Topic:\n"
                   + " Read the webpage title/heading and understand the main topic or purpose it conveys. This sets the context for evaluating relevance.\n"
                   + " 2.Analyze Content:\n"
                   + " Read the content of the webpage thoroughly to assess if it directly relates to the main topic indicated by the title/heading."
                   + " Look for keywords, key phrases, or themes that should be present based on the title/heading.\n"
                   + " 3.Check for Consistency:\n"
                   + " Evaluate if the content maintains consistency with the main topic throughout. Ensure that there are no significant digressions or irrelevant sections.\n"
                   + " 4.Evaluate Relevance:\n"
                   + " Determine the extent to which the content aligns with the main topic. Content should be directly relevant and contribute to the understanding or exploration of the main theme."
                   + " Identify any sections or information that might be off-topic or detract from the main purpose of the webpage.\n"
                   + " 5.Consider Audience Intent:\n"
                   + " Consider the intended audience and their expectations when assessing relevance. The content should meet the needs and interests of the target audience based on the title/heading.\n"
                   + " 6.Provide Feedback or Adjustments:\n"
                   + " If you find content that is irrelevant or off-topic, provide feedback to the content to improve the overall quality and SEO (Search Engine Optimization) effectiveness of the webpage.";

                var responseText = await azureOpenAiService.GenerateText(userMessagePrompt, responseFormat);
                if (responseText != null)
                {
                    var responseDeserialized = JsonConvert.DeserializeObject<RelevanceToTheTopic>(responseText);
                    if (responseDeserialized != null)
                    {
                        RelevanceToTheTopic relevanceToTheTopic = new RelevanceToTheTopic()
                        {
                            ContentQualityId = contentQualityId,
                            UnderstandingOfTopic = responseDeserialized.UnderstandingOfTopic,
                            AnalysisOfContent = responseDeserialized.AnalysisOfContent,
                            ConsistencyCheck = responseDeserialized.ConsistencyCheck,
                            RelevanceAssessment = responseDeserialized.RelevanceAssessment,
                            ConsiderationOfAudienceIntent = responseDeserialized.ConsiderationOfAudienceIntent,
                            FeedbackOfContent = responseDeserialized.FeedbackOfContent,
                            TotalScore = responseDeserialized.TotalScore
                        };

                        return await relevanceToTheTopicService?.Create("RelevanceToTheTopic", relevanceToTheTopic);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Accuracy and Credibility: Evaluate the accuracy and reliability of the information
        /// presented. Look for authoritative sources, citations, and evidence to support claims.
        /// Content from reputable sources tends to be more trustworthy. 
        /// </summary>
        private async Task<bool> GetAccuracyAndCredibility(Project project, HtmlDocument doc, string contentQualityId)
        {
            AccuracyAndCredibility accuracyAndCredibility = new AccuracyAndCredibility();
            // Extract text content from the webpage
            //string textContent = doc.DocumentNode.InnerText;
            HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//body");
            string textContent = bodyNode.InnerHtml;

            accuracyAndCredibility.ContentQualityId = contentQualityId;

            accuracyAndCredibility.WhoIsDoaminScore = project.WhoIsDomain is null ? 0 : 1; //TODO: Write logic to calculate Score

            // Check for authoritative sources (example: presence of .edu or .gov domains)
            accuracyAndCredibility.HasCitationsInParentheses = textContent.Contains(".edu") || textContent.Contains(".gov");

            //AuthoredByExperts method checks for mentions of expert names or titles within the text content.
            accuracyAndCredibility.AuthoredByExperts = AuthoredByExperts(textContent);

            // Check for citations and evidence (example: presence of references, footnotes)
            accuracyAndCredibility.HasCitations = Regex.IsMatch(textContent, @"\[\d+\]"); // Assuming citations are in the format [1], [2], etc.

            accuracyAndCredibility.CitationCount = Regex.Matches(textContent, @"\[\d+\]").Count;

            // Assuming references section is delimited by certain markers (e.g., <references>...</references>)
            Match referencesSectionMatch = Regex.Match(textContent, @"<references>(.*?)</references>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (referencesSectionMatch.Success)
            {
                accuracyAndCredibility.ReferencesSectionContent = referencesSectionMatch.Groups[1].Value;
                // Now you can further process the references section to extract citation information
            }

            accuracyAndCredibility.HasCitationsInParentheses = Regex.IsMatch(textContent, @"\(\[\d+\]\)");

            // Evaluate reputation of sources (example: check domain reputation, credibility of mentioned organizations)
            accuracyAndCredibility.ReputationScore = await EvaluateReputation(project, doc);

            // Calculate accuracy and credibility score
            double accuracyCredibilityScore = CalculateAccuracyCredibilityScore(accuracyAndCredibility.HasAuthoritativeSources, accuracyAndCredibility.HasCitations, accuracyAndCredibility.ReputationScore);

            accuracyAndCredibility.AccuracyCredibilityScore = accuracyCredibilityScore.ToString();
            try
            {
                return await accuracyAndCredibilityService?.Create("accuracyAndCredibility", accuracyAndCredibility);

            }
            catch (Exception)
            {
                return false;
            }

        }

        //AuthoredByExperts method checks for mentions of expert names or titles within the text content.
        private static bool AuthoredByExperts(string textContent)
        {
            // Check for mentions of expert names or titles in the content
            string[] expertKeywords = { "expert", "specialist", "authority", "professor",
                                        "scholar", "researcher", "academic", "consultant",
                                        "analyst", "scientist", "doctor", "advisor",
                                        "mentor", "lecturer", "instructor", "teacher" };

            foreach (var keyword in expertKeywords)
            {
                if (textContent.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Clarity and Readability: Assess the clarity and readability of the content. Is the language
        /// clear and concise? Are complex concepts explained in a way that's easy for the target
        /// audience to understand? Proper grammar, spelling, and formatting contribute to readability. 
        ///Text Analysis:
        ///Analyze the extracted text content to evaluate clarity and readability. You can use natural language processing (NLP) techniques and
        ///libraries like SharpNLP or //ML.NET to perform text analysis tasks such as:
        ///a.Language Clarity:
        ///- Check for the clarity and conciseness of the language used in the content. You can assess readability using readability metrics
        ///like Flesch-Kincaid, //Coleman-Liau, or Gunning Fog index.
        ///b. Explanation of Complex Concepts:
        ///- Identify and analyze the explanation of complex concepts in the content. You can use NLP techniques to detect and evaluate the
        ///complexity of sentences or phrases.
        ///c. Grammar and Spelling:
        ///- Please do spelling checking and grammar checking to identify and correct grammar and spelling errors in the content.
        ///d. Formatting:
        ///- Assess the formatting of the content, including proper use of headings, paragraphs, lists, and other formatting elements. 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private async Task<bool> GetClarityAndReadability(HtmlDocument doc, string contentQualityId)
        {
            HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//body");
            string textContent = bodyNode.InnerHtml;
            if (string.IsNullOrEmpty(textContent))
            {
                var responseFormat = "Your response always in below JSON Format.\n"
                          + " 'ClarityAndReadability' : {"
                          + " 'languageClarity': null,"
                          + " 'explanationOfComplexConcepts': null,"
                          + " 'grammarAndSpelling': null,"
                          + " 'formatting': null  "
                          + " 'TotalScore' : 0"
                          + "}";

                var userMessagePrompt = "Based on the points provided below, could you analyze the text in bullet points, keeping them simple and concise within 30 words each? "
                          + "Also, calculate the TotalScore based on all the points.\n"
                          + " a. Language Clarity:"
                          + "  - Check for the clarity and conciseness of the language used in the content. You can assess readability using readability metrics like Flesch-Kincaid, Coleman-Liau, or Gunning Fog index."
                          + " b. Explanation of Complex Concepts:"
                          + "  - Identify and analyze the explanation of complex concepts in the content. You can use NLP techniques to detect and evaluate the complexity of sentences or phrases."
                          + " c. Grammar and Spelling:"
                          + "  - Please do spelling and grammar checking to identify and correct grammar and spelling errors in the content."
                          + " d. Formatting:"
                          + "  - Assess the formatting of the content, including proper use of headings, paragraphs, lists, and other formatting elements.\n"
                          + textContent;

                var responseText = await azureOpenAiService.GenerateText(userMessagePrompt, responseFormat);
                if (responseText != null)
                {
                    var responseDeserialized = JsonConvert.DeserializeObject<ClarityAndReadability>(responseText);

                    if (responseDeserialized != null)
                    {
                        ClarityAndReadability clarityAndReadability = new ClarityAndReadability()
                        {
                            ContentQualityId = contentQualityId,
                            LanguageClarity = responseDeserialized.LanguageClarity,
                            ExplanationOfComplexConcepts = responseDeserialized.ExplanationOfComplexConcepts,
                            GrammarAndSpelling = responseDeserialized.GrammarAndSpelling,
                            Formatting = responseDeserialized.Formatting,
                            TotalScore = responseDeserialized.TotalScore
                        };

                        return await clarityAndReadabilityService?.Create("ClarityAndReadability", clarityAndReadability);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Depth and Breadth of Coverage: Consider the depth and breadth of coverage on the topic.
        /// High-quality content provides comprehensive information that addresses key aspects of the subject 
        /// matter. It may include examples, case studies, or multimedia elements to enhance understanding. 
        /// </summary>
        private async Task<bool> GetDepthAndBreadthOfCoverage(HtmlDocument doc, string contentQualityId)
        {
            HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//body");
            string textContent = bodyNode.InnerHtml;
            if (string.IsNullOrEmpty(textContent))
            {
                var responseFormat = "Your response always in below JSON Format.\n"
                          + "'DepthAndBreadthOfCoverage':{"
                          + "  'keyAspects': null,"
                          + "  'contentAnalysis': null,"
                          + "  'depthOfCoverage': null,"
                          + "  'breadthOfCoverage': null,"
                          + "  'supportingElements': null,"
                          + "  'feedback': null,"
                          + "  'totalScore': 0"
                          + "}";

                var userMessagePrompt = "Please evaluate the depth and breadth of coverage on a topic/content, consider the following steps:\n"
                          + " 1.Define Key Aspects:\n"
                          + "  - Identify the key aspects or components relevant to the topic. This could include important concepts, subtopics, or areas of discussion.\n"
                          + " 2.Content Analysis:\n"
                          + "  - Review the content to assess how well it covers each key aspect. Look for detailed explanations, examples, and supporting information.\n"
                          + " 3.Depth of Coverage:\n"
                          + "  - Evaluate the depth of coverage for each key aspect. High-quality content delves into the topic with sufficient detail, providing in-depth information and insights.\n"
                          + " 4.Breadth of Coverage:\n"
                          + "  - Consider the breadth of coverage across different areas related to the topic. Quality content should not be overly narrow but should encompass a range of relevant subtopics or perspectives.\n"
                          + " 5.Inclusion of Supporting Elements:\n"
                          + "  - Assess if the content includes supporting elements such as examples, case studies, statistics, or multimedia elements (like images, videos, infographics).\n"
                          + "  - These elements enhance understanding and engagement, providing a richer experience for the audience.\n"
                          + " 6.Comparison with Competitors:\n"
                          + "  - Optionally, compare the depth and breadth of coverage with similar content from competitors or authoritative sources.\n"
                          + "  - Identify areas where your content excels or where improvements can be made based on this comparison.\n"
                          + " 7.Provide Feedback:\n"
                          + "  - If you find content coverage is not covering the depth and breadth of the topc.\n"
                          + "  - Please provides feedback to improve the overall quality and SEO (Search Engine Optimization) effectiveness of the webpage.\n"
                          + textContent;

                var responseText = await azureOpenAiService.GenerateText(userMessagePrompt, responseFormat);
                if (responseText != null)
                {
                    var responseDeserialized = JsonConvert.DeserializeObject<DepthAndBreadthOfCoverage>(responseText);
                    if (responseDeserialized != null)
                    {
                        DepthAndBreadthOfCoverage depthAndBreadthOfCoverage = new DepthAndBreadthOfCoverage()
                        {
                            ContentQualityId = contentQualityId,
                            KeyAspects = responseDeserialized.KeyAspects,
                            ContentAnalysis = responseDeserialized.ContentAnalysis,
                            DepthOfCoverage = responseDeserialized.DepthOfCoverage,
                            BreadthOfCoverage = responseDeserialized.BreadthOfCoverage,
                            SupportingElements = responseDeserialized.SupportingElements,
                            Feedback = responseDeserialized.Feedback,
                            TotalScore = responseDeserialized.TotalScore,
                        };

                        return await depthAndBreadthOfCoverageService?.Create("DepthAndBreadthOfCoverage ", depthAndBreadthOfCoverage);

                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Uniqueness and Originality: Determine whether the content offers a unique perspective or adds 
        /// value beyond what's available elsewhere. Original content that offers fresh insights or presents 
        /// information in a novel way is more likely to be considered high quality. 
        /// </summary>
        private string GetUniquenessAndOriginality(HtmlDocument doc)
        {
            bool isPlagiarismRequire = false;
            if (isPlagiarismRequire)
            {
                HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//body");
                string textContent = bodyNode.InnerText;
                if (!string.IsNullOrEmpty(textContent))
                {

                    var xmlElement = Common.CopyscapeApi.text_search_internet(textContent, 0);
                    if (xmlElement != null)
                    {
                        return xmlElement.InnerText; // TODO: Extract the proper information from innnerText
                    }
                }
            }

            return "Plagiarism Not Checked";
        }

        /// <summary>
        /// Engagement and Interactivity: Evaluate how engaging the content is for the audience. 
        /// Does it encourage interaction, such as through comments, social sharing, or interactive 
        /// elements? Engaging content tends to keep users on the page longer and encourages them to 
        /// take action. 
        /// </summary>
        private string GetEngagementAndInteractivity(HtmlDocument doc)
        {
            return "Engagement And Interactivity can seen after Google account access from google Analytics."; //TODO: Integrate with google Api to get the
                                                                                                               //information from google Analytics
        }

        /// <summary>
        /// User Experience (UX): Consider the overall user experience of the webpage. Is the layout 
        /// intuitive and easy to navigate? Does the content load quickly and display properly on 
        /// various devices and screen sizes? A positive UX enhances the perceived quality of the content. 
        /// </summary>
        private async Task<string> GetUserExperience(HtmlDocument doc)
        {
            var isReuiredPageInsight = false;
            if (isReuiredPageInsight)
            {
                await UXfunction(); // TODO: Integration with Google API for UX and Page
                                    // loading Speed or Pageinsight details 
            }
            return JsonConvert.SerializeObject("");
        }

        /// <summary>
        /// Reputation and Authority: Assess the reputation and authority of the website or author publishing 
        /// the content. Established websites with a history of producing high-quality content are more likely 
        /// to maintain consistent standards. 
        /// </summary>
        private async Task<bool> GetReputationAndAuthority(Project project, HtmlDocument doc, string contentQualityId)
        {
            //TODO: Write a logic for the Reputation And Authority of the page content
            double reputationScore = 0;
            string domain = project.URL;
            string author = "Auther Name";

            ReputationAndAuthority reputationAndAuthority = new ReputationAndAuthority();
            // Example: Calculate domain age and assign reputation score
            var domainAgeInYears = GetDomainAge(domain);
            reputationScore += domainAgeInYears * 0.5;  // Domain age contributes to reputation
            reputationAndAuthority.DomainAgeInYears = Convert.ToString(domainAgeInYears);

            // Example: Analyze backlinks and assign reputation score
            List<string> backlinkList = GetBacklinkCount(domain);
            reputationScore += backlinkList.Count() * 0.2;  // Backlinks contribute to reputation
            reputationAndAuthority.BacklinkCount = JsonConvert.SerializeObject(backlinkList);

            // Example: Analyze social signals and assign reputation score
            int socialEngagement = GetSocialEngagement(domain);
            reputationScore += socialEngagement * 0.3;  // Social signals contribute to reputation
            reputationAndAuthority.SocialEngagement = Convert.ToString(socialEngagement);

            // Example: Evaluate author expertise and assign authority score
            double authorExpertise = GetAuthorExpertise(author);
            reputationScore += authorExpertise * 0.5;  // Author expertise contributes to authority
            reputationAndAuthority.AuthorExpertise = Convert.ToString(authorExpertise);
            reputationAndAuthority.ReputationScore = reputationScore;

            try
            {
                return await reputationAndAuthorityService?.Create("ReputationAndAuthority", reputationAndAuthority);

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Purpose and Intent: Consider the intended purpose of the content. Is it educational, informative, 
        /// entertaining, or promotional? High-quality content effectively fulfills its intended purpose 
        /// and meets the needs of the target audience. 
        /// </summary>
        private async Task<bool> GetPurposeAndIntent(HtmlDocument doc, string contentQualityId)
        {
            HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//body");
            string textContent = bodyNode.InnerHtml;
            if (string.IsNullOrEmpty(textContent))
            {
                PurposeAndIntent purposeAndIntent = new PurposeAndIntent();

                // Check if the content contains keywords indicative of its purpose
                purposeAndIntent.EducationAndInformativeCount = ContainsKeywords(textContent, new string[] { "educational", "informative", "tutorials", "guides", "articles", "research papers", "FAQs", "how-to content" });
                purposeAndIntent.EntertainingCount = ContainsKeywords(textContent, new string[] { "entertaining", "fun", "games", "videos", "quizzes", "interactive experiences", "humor content", "storytelling" });
                purposeAndIntent.PromotionalCount = ContainsKeywords(textContent, new string[] { "promotional", "advertisement", "promotional offers", "product descriptions", "testimonials", " call-to-action" });
                purposeAndIntent.TransactionalAndECommerceCount = ContainsKeywords(textContent, new string[] { "listings,", "shopping carts", "checkout", "payment gateways", "order confirmation" });
                purposeAndIntent.NewsAndUpdatesCount = ContainsKeywords(textContent, new string[] { "news articles", "press releases", "newsletters" });
                purposeAndIntent.SocialInteractionAndCommunityBuildingCount = ContainsKeywords(textContent, new string[] { "forums", "comment sections", "social media feeds", "user-generated content", "community events" });
                purposeAndIntent.EducationalInstitutionsTrainingCount = ContainsKeywords(textContent, new string[] { "course materials", "syllabi", "schedules", "admissions information", "academic resources", "student services" });
                purposeAndIntent.LegalAndPolicyCount = ContainsKeywords(textContent, new string[] { "legal terms", "policies", "terms of service", "privacy policies", "disclaimers", " copyright information", "compliance requirements" });
                purposeAndIntent.ReviewsAndTestimonialsCount = ContainsKeywords(textContent, new string[] { "reviews", "testimonials", "ratings", "case studies", "success stories", "feedback", "services", "experiences" });
                purposeAndIntent.ResourceAndReferenceCount = ContainsKeywords(textContent, new string[] { "resource", "reference for users", "providing information", "data", "statistics", "charts", "graphs", "infographics", "reference materials" });
                purposeAndIntent.SupportAndHelp = ContainsKeywords(textContent, new string[] { "support", "assistance", "troubleshooting guides", "FAQs", "technical documentation", "user manuals", "customer service information" });
                purposeAndIntent.BlogAndOpinion = ContainsKeywords(textContent, new string[] { "blog posts", "articles", "editorials", "opinions", "analysis", "commentary", " thought leadership" });

                try
                {
                    return await purposeAndIntentService?.Create("PurposeAndIntent", purposeAndIntent);

                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }


        /// <summary>
        /// Feedback and Metrics: Gather feedback from users, such as through comments, ratings, or surveys. 
        /// Analyze metrics such as time on page, bounce rate, and social shares to gauge user engagement 
        /// and satisfaction with the content. 
        /// </summary>
        private bool GetFeedbackAndMetrics(HtmlDocument doc, string contentQualityId)
        {
            //return "Feedback And Metrics can seen after Google account access from google Analytics."; //TODO: Integrate with google Api to get the
            return false;                                                                              //information from google Analytics
        }
        #endregion
    }
}
