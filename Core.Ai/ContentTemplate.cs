using Core.Shared.Entities;
using System.Reflection.Metadata;

namespace Core.Ai
{
    public class ContentTemplate
    {
        public static string? GetPrimaryKeywordsTemplate(Blog request)
        {
            return $@"You are an expert SEO strategist. Based on the parameters provided below, generate a list of **primary keywords** for MockTestLab.com. Ensure the keywords are relevant, highly searched, and align with the SEO goals.

                    Parameters:
                    1. **Target Audience**: Students preparing for competitive exams  
   
                    2. **Topic or Niche**: {{title_of_content}}  

                    3. **Primary Focus**: {{title_of_content}} 

                    4. **Location (if applicable)**: {{target_location}} 

                    5. **Competitors (optional)**: {{competitor_urls}}  

                    6. **Search Intent**: {{search_intent}}  
                       
                    7. **Additional Notes or Focus Areas**: {{additional_notes}}  

                    ### Instructions:
                    1. Generate **3-5 primary keywords** that are concise, specific, and optimized for high search volume.
                    2. Prioritize long-tail keywords when possible for better targeting.
                    3. Ensure the keywords are highly relevant to the content and audience.
                    4. Provide variations that cover different search intents (informational, transactional, etc.).";
        }
        public static string? GetSecondaryKeywordsTemplate(Blog request)
        {
            return $@"You are an expert SEO strategist. Based on the parameters provided below, generate a list of **secondary keywords** for MockTestLab.com. These keywords should complement the primary keywords and focus on related topics or broader and more specific variations to enhance SEO performance.

                    Parameters:
                    1. **Primary Keywords**: {{primary_keywords}}  

                    2. **Target Audience**: {{target_audience}}  

                    3. **Topic or Niche**: {{title_of_content}}  

                    4. **Location (if applicable)**: {{target_location}}  

                    5. **Competitors (optional)**: {{competitor_urls}}  
                       
                    6. **Search Intent**: {{search_intent}}  

                    7. **Additional Notes or Focus Areas**: {{additional_notes}}  

                    ### Instructions:
                    1. Generate **5-10 secondary keywords** that are broader, more specific, or closely related to the primary keywords.  
                    2. Ensure secondary keywords target different aspects of the topic, including long-tail variations.  
                    3. Provide suggestions for keywords that cater to related search intents (e.g., related questions, tools, or platforms).";
        }
        public static string? GetMetaTagsTemplate(Blog request)
        {
            return $@"Generate SEO meta tags for a blog with the following details:

                     - **Blog Title**: ""{{{{title_of_content}}}}""  
                     - **Content Type**: ""{{{{content_type}}}}"" (e.g., guide, tutorial, listicle, case study)  
                     - **Primary Keywords**: ""{{{{primary_keywords}}}}"" (Most important SEO keywords, separated by commas)  
                     - **Secondary Keywords**: ""{{{{secondary_keywords}}}}"" (Additional supporting keywords, separated by commas)  
                     
                     #### **Meta Tags to Generate**:
                     1. **Meta Title** (Max 60 characters):  
                        - Must be compelling, keyword-rich, and include the primary keyword.  
                     
                     2. **Meta Description** (150-160 characters):  
                        - Engaging, SEO-optimized, includes primary keyword and at least one secondary keyword.  
                     
                     3. **Open Graph Tags (for Facebook & LinkedIn)**:  
                        - `og:title`: Reflects the blog title with primary keyword.  
                        - `og:description`: A compelling summary, optimized for social sharing.  
                        - `og:image`: URL for the featured blog image.  
                        - `og:url`: Canonical blog post URL.  
                        - `og:type`: ""article"" to indicate a blog post.  
                        - `og:site_name`: ""MockTestLab""  
                     
                     4. **Twitter Meta Tags**:  
                        - `twitter:card`: ""summary_large_image"" for better engagement.  
                        - `twitter:title`: Same as Open Graph title.  
                        - `twitter:description`: Same as Open Graph description.  
                        - `twitter:image`: URL for the blog’s featured image.  
                     
                     5. **Canonical URL**:  
                        - Ensures no duplicate content issues.  
                     
                     6. **Robots Meta Tag**:  
                        - Controls search engine indexing, set to ""index, follow"" by default.  
                     
                     Ensure all meta tags follow proper **HTML5 format** and are optimized for **SEO & social media sharing**.";
        }
        public static string? GetContentTemplate(Blog request)
        {
            return $@"Title: ""{{title_of_content}}""
                    Write a {{content_type}} for MockTestLab.com targeting the audience of {{target_audience}}. The primary focus keyword is ""{{primary_keywords}},"" and the secondary keywords are {{secondary_keywords}}. 
                    Structure the content as follows:
                    1. Start with a compelling introduction that includes the primary keyword within the first 100 words.
                    2. Use clear headings and subheadings (use H1 for the main title and H2/H3 for subheadings) that include the primary or secondary keywords where appropriate.
                    3. Incorporate short paragraphs, bullet points, and lists for easy readability.
                    4. Add a conclusion with a strong call-to-action like ""{{cta_action}}.""

                    Word count should be between {{word_count_min}} and {{word_count_max}}.

                    Additional requirements:
                    - The tone should be {{tone_of_voice}} (e.g., friendly, professional, or conversational).
                    - The content must be 100% original, plagiarism-free, and highly engaging.
                    - Suggest internal links related to {{internal_link_topics}} and add one external authoritative link about {{external_link_topic}}.
                    - Provide a meta description under 160 characters using the primary keyword and a call-to-action.

                    Target audience location: {{target_location}} (if applicable).
                    Optional notes: {{additional_notes}}
                    Example competitor references or inspiration: {{competitor_urls}} (if provided).
                    Make the content SEO-friendly and engaging to improve search engine rankings for MockTestLab.com.";
        }

        public static string FillContent(string templateContent, Dictionary<string, string> values)
        {
            foreach (var placeholder in values)
            {
                // Ensure the placeholder has the unique prefix
                templateContent = templateContent.Replace($"{{{placeholder.Key}}}", placeholder.Value);
            }
            return templateContent;
        }

        public static Dictionary<string, string> GetPlaceHolderKeysAndValues(Blog request)
        {
            return new Dictionary<string, string>
                    {
                        { "title_of_content", request.Title },
                        { "content_type", request.ContentType },
                        { "target_audience", request.TargetAudience },
                        { "search_intent", request.SearchIntent},
                        { "cta_action", request.CtaAction },
                        { "target_location", request.TargetLocation },
                        { "tone_of_voice", request.ToneOfVoice },
                        { "word_count_min", request.WordCountMin },
                        { "word_count_max", request.WordCountMax },
                        { "additional_notes", request.AdditionalNotes },
                        { "competitor_urls", request.CompetitorUrls },
                        { "primary_keywords", request.PrimaryKeyword },
                        { "secondary_keywords", request.SecondaryKeywords }
                    };
        }
        public static string? GetResponseKeywordTemplate()
        {
            return $@"[keyword_1], [keyword_2], [keyword_3], [keyword_4], [keyword_5]";
        }
        public static string? GetResponseTemplate()
        {
            return $@"# {{title}}

                    ## Table of Contents  
                    1. [Introduction](#introduction)  
                    2. [Why Are Mock Tests Important?](#why-are-mock-tests-important)  
                    3. [How to Analyze Your Mock Test Performance](#how-to-analyze-your-mock-test-performance)  
                    4. [10 Proven Tips for Mock Test Success](#10-proven-tips-for-mock-test-success)  
                    5. [FAQs on Mock Test Preparation](#faqs-on-mock-test-preparation)  
                    6. [Final Thoughts](#final-thoughts)  
                    7. [Additional Resources](#additional-resources)  
                    ---
                    ## Introduction  
                    {{introduction}}  

                    ![Mock Test Preparation](#image-placeholder)  

                    ## Why Are Mock Tests Important?  
                    Mock tests play a vital role in exam preparation by simulating real exam conditions. They help candidates refine their test-taking strategies, manage stress, and pinpoint areas that need improvement.  

                    ### **Key Benefits:**  
                    ✅ **Better Time Management** – Learn how to allocate time effectively.  
                    ✅ **Understanding Question Patterns** – Familiarize yourself with different question formats.  
                    ✅ **Boosts Confidence** – Reduces anxiety and improves self-assurance before the actual exam.  

                    ## How to Analyze Your Mock Test Performance  
                    To get the most out of mock tests, it's essential to review and analyze your performance.  

                    ### **Key Steps:**  
                    1. **Review Mistakes:** Go through incorrect answers and understand why you got them wrong.  
                    2. **Focus on Weak Areas:** Identify subjects or topics that need more attention.  
                    3. **Track Your Progress:** Keep a record of your scores to measure improvement over time.  

                    > 💡 **Pro Tip:** Use a performance tracking sheet or an online test analytics tool for better insights.  

                    ## **10 Proven Tips for Mock Test Success**  
                    Follow these expert-recommended strategies to improve your test scores.  

                    ### **1. Take Tests in a Distraction-Free Environment**  
                    Create a quiet and comfortable study space to maintain focus while attempting mock tests.  

                    ### **2. Stick to the Exam Time Limit**  
                    Practice time-bound tests to build speed and accuracy, just like the real exam.  

                    ### **3. Use the Elimination Method**  
                    For multiple-choice questions, eliminate incorrect answers to improve your chances of selecting the right one.  

                    ### **4. Prioritize Difficult Sections First**  
                    Attempt tougher sections when your mind is fresh to maximize efficiency.  

                    ### **5. Simulate the Actual Exam Conditions**  
                    Take full-length tests in a single sitting without distractions to build endurance.  

                    ### **6. Review Every Attempted Question**  
                    Analyze your mistakes and learn why you made them to avoid repeating errors.  

                    ### **7. Take Notes on Recurring Mistakes**  
                    Maintain a mistake journal to track errors and work on areas needing improvement.  

                    ### **8. Solve Previous Year Papers**  
                    Practicing past exam papers can help you recognize frequently asked questions.  

                    ### **9. Improve Time Allocation for Each Section**  
                    Develop a time-management strategy to complete all sections without rushing.  

                    ### **10. Stay Consistent with Practice**  
                    Mock test success requires regular practice—don’t wait until the last minute!  

                    ## **FAQs on Mock Test Preparation**  
                    Here are some frequently asked questions about mock test preparation:  

                    **Q1: How many mock tests should I take before the exam?**  
                    A: Ideally, take **at least 8-10 full-length mock tests** to get comfortable with the exam format.  

                    **Q2: How do I improve my weak subjects?**  
                    A: Identify problem areas, review concepts, and take **topic-wise mock tests** for improvement.  

                    **Q3: Is it necessary to take mock tests under timed conditions?**  
                    A: Yes! Timed practice helps you build speed and confidence.  

                    **Q4: Should I review incorrect answers after a mock test?**  
                    A: Absolutely! **Understanding mistakes** ensures you don’t repeat them in the actual exam.  

                    **Q5: Can mock tests predict my real exam score?**  
                    A: While they provide a good estimate, your actual score depends on multiple factors like stress, question difficulty, and last-minute revisions.  

                    ## **Final Thoughts**  
                    Mock tests are a powerful tool for exam success. By following these 10 tips, you can significantly improve your accuracy, confidence, and overall performance.  

                    ### **🚀 Ready to get started? [Take free mock tests at MockTestLab.com](#cta-link)**  

                    ## **Additional Resources**  
                    Looking for more exam preparation strategies? Check out our expert guides below:  

                    📌 **[How to Manage Exam Stress](#internal-link1)**  
                    📌 **[Best Time Management Strategies for Exams](#internal-link2)**  
                    📌 **[How to Improve Your Memory for Study](#internal-link3)**  

                    ---

                    ### **How This Works for AI Content Generation?**  
                    ✅ **Uses Markdown Syntax** – Helps AI tools structure content dynamically.  
                    ✅ **SEO-Friendly Format** – Uses **headings, lists, bold, and links** for better ranking.  
                    ✅ **Easy to Customize** – Replace placeholders (`{{title}}`, `{{introduction}}`, etc.) with dynamic values.  
                    ✅ **Includes FAQs** – Boosts Google search ranking by answering common queries.  
                    ✅ **Image Placeholder** – AI can generate & insert relevant images dynamically.  
                    ✅ **Schema-Optimized** – Helps search engines understand the content better.  

                    ---

                    ### **Need More Customizations?**  
                    Let me know if you need:  
                    📌 **More FAQs**  
                    📌 **Interactive tables**  
                    📌 **Infographic placeholders**  
                    📌 **SEO Schema Markup**  

                    🚀 **Let’s create AI-optimized content that ranks higher and converts better!** 🚀";
        }

        internal static string? GetResponseMetaTagsTemplate()
        {
            return $@" <meta name=""title"" content=""10 Proven Tips for Mock Test Preparation | MockTestLab"">
                       <meta name=""description"" content=""Discover 10 proven tips for mock test preparation to improve your scores. Get expert strategies at MockTestLab.com!"">
                       <meta name=""keywords"" content=""mock tests, exam preparation, test-taking strategies, study tips"">
                       <meta name=""robots"" content=""index, follow"">
    
                       <meta property=""og:title"" content=""10 Proven Tips for Mock Test Preparation"">
                       <meta property=""og:description"" content=""Want to ace your mock tests? Discover these expert-backed strategies to improve your scores at MockTestLab.com!"">
                       <meta property=""og:image"" content=""https://mocktestlab.com/assets/mock-test-tips.jpg"">
                       <meta property=""og:url"" content=""https://mocktestlab.com/blog/mock-test-tips"">
                       <meta property=""og:type"" content=""article"">
                       <meta property=""og:site_name"" content=""MockTestLab"">
    
                       <meta name=""twitter:card"" content=""summary_large_image"">
                       <meta name=""twitter:title"" content=""10 Proven Tips for Mock Test Preparation"">
                       <meta name=""twitter:description"" content=""Struggling with mock tests? Get these expert tips to improve your exam scores at MockTestLab.com!"">
                       <meta name=""twitter:image"" content=""https://mocktestlab.com/assets/mock-test-tips.jpg"">
    
                       <link rel=""canonical"" href=""https://mocktestlab.com/blog/mock-test-tips"">
                       <meta name=""author"" content=""MockTestLab Editorial Team"">";
        }
    }
}
