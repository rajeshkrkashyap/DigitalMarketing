using Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Ai
{
    public class CompetitorArticle
    {
        public async Task<string?> AiGeneratePrimaryKeywords(Blog request)
        {
            if (string.IsNullOrEmpty(request.Title)
                || string.IsNullOrEmpty(request.ContentType)
                || string.IsNullOrEmpty(request.SearchIntent)
                || string.IsNullOrEmpty(request.TargetAudience)
                || string.IsNullOrEmpty(request.TargetLocation)
                || string.IsNullOrEmpty(request.ToneOfVoice)
                || string.IsNullOrEmpty(request.WordCountMin)
                || string.IsNullOrEmpty(request.WordCountMax))
            {
                return "";
            }
            if (!string.IsNullOrEmpty(request.Title))
            {
                var blogPromptTemplate = ContentTemplate.GetPrimaryKeywordsTemplate(request);
                var responseTemplate = ContentTemplate.GetResponseKeywordTemplate();
                var placeHolderValues = ContentTemplate.GetPlaceHolderKeysAndValues(request);
                var requestFormat = ContentTemplate.FillContent(blogPromptTemplate, placeHolderValues);
                //AiCall _aiCall = new AiCall(UserDetail, _azureOpenAiService);
                var content = await CallToAi(requestFormat, responseTemplate);
                return content;
            }
            return "";
        }

        public async Task<string?> AiGenerateSecondaryKeywords(Blog request)
        {
            if (string.IsNullOrEmpty(request.Title)
                || string.IsNullOrEmpty(request.ContentType)
                || string.IsNullOrEmpty(request.SearchIntent)
                || string.IsNullOrEmpty(request.TargetAudience)
                || string.IsNullOrEmpty(request.TargetLocation)
                || string.IsNullOrEmpty(request.ToneOfVoice)
                || string.IsNullOrEmpty(request.WordCountMin)
                || string.IsNullOrEmpty(request.WordCountMax))
            {
                return "";
            }

            if (!string.IsNullOrEmpty(request.Title))
            {
                var blogPromptTemplate = ContentTemplate.GetSecondaryKeywordsTemplate(request);
                var responseTemplate = ContentTemplate.GetResponseKeywordTemplate();
                var placeHolderValues = ContentTemplate.GetPlaceHolderKeysAndValues(request);
                var requestFormat = ContentTemplate.FillContent(blogPromptTemplate, placeHolderValues);
                //AzureCall _azureCall = new AzureCall(UserDetail, _azureOpenAiService);
                var content = await CallToAi(requestFormat, responseTemplate);
                return content;
            }
            return "";
        }

        public async Task<string?> AiGenerateContent(Blog request)
        {
            if (string.IsNullOrEmpty(request.Title)
                || string.IsNullOrEmpty(request.ContentType)
                || string.IsNullOrEmpty(request.SearchIntent)
                || string.IsNullOrEmpty(request.TargetAudience)
                || string.IsNullOrEmpty(request.TargetLocation)
                || string.IsNullOrEmpty(request.ToneOfVoice)
                || string.IsNullOrEmpty(request.WordCountMin)
                || string.IsNullOrEmpty(request.WordCountMax))
            {
                return "";
            }
            if (!string.IsNullOrEmpty(request.Title))
            {
                var blogPromptTemplateContent = ContentTemplate.GetContentTemplate(request);
                var responseTemplate = ContentTemplate.GetResponseTemplate();
                var placeHolderValues = ContentTemplate.GetPlaceHolderKeysAndValues(request);
                var requestFormat = ContentTemplate.FillContent(blogPromptTemplateContent, placeHolderValues);
                //AzureCall _azureCall = new AzureCall(UserDetail, _azureOpenAiService);
                var content = await CallToAi(requestFormat, responseTemplate);
                return content;
            }
            return "";
        }

        public async Task<string?> AiGenerateMetaTags(Blog request)
        {
            if (string.IsNullOrEmpty(request.Title)
                || string.IsNullOrEmpty(request.ContentType)
                || string.IsNullOrEmpty(request.PrimaryKeyword)
                || string.IsNullOrEmpty(request.SecondaryKeywords))
            {
                return "";
            }
            if (!string.IsNullOrEmpty(request.Title))
            {
                var requestTemplate = ContentTemplate.GetMetaTagsTemplate(request);
                var keyValues = ContentTemplate.GetPlaceHolderKeysAndValues(request);
                var requestFormat = ContentTemplate.FillContent(requestTemplate, keyValues);
                var responseTemplate = ContentTemplate.GetResponseMetaTagsTemplate();

                //AzureCall _azureCall = new AzureCall(UserDetail, _azureOpenAiService);
                var content = await CallToAi(requestFormat, responseTemplate);
                var stringMetaTags = ExtractMetaTags(content);
                return stringMetaTags;
            }
            return "";
        }

        private async Task<string?> CallToAi(string requestFilledTemplate, string responseTemplate)
        {
            return await AiCall.CallToAiForQuestion(requestFilledTemplate, responseTemplate);
        }

        static string ExtractMetaTags(string input)
        {
            // Regex pattern to match meta tags and link tags
            string metaTagPattern = @"<meta\s[^>]+>|<link\s[^>]+>";
            List<string> metaTags = new List<string>();

            // Extract all meta and link tags
            MatchCollection matches = Regex.Matches(input, metaTagPattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                metaTags.Add(match.Value);
            }

            // Remove meta and link tags from the original content
            string cleanedText = Regex.Replace(input, metaTagPattern, "").Trim();

            var stringMetaTags = "";
            foreach (var item in metaTags)
            {
                stringMetaTags += "\n" + item;
            }
            return stringMetaTags;
        }
    }
}
