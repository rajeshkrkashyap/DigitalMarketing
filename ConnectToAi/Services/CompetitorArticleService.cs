using Core.Ai;
using Core.Shared;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConnectToAi.Services
{
    public class CompetitorArticleService: BaseService 
    {
        readonly HttpClient httpClient;
        public CompetitorArticleService(ConfigService configService) : base(configService)
        {
            httpClient = new HttpClient();
        }
        
        public async Task<string?> AiGeneratePrimaryKeywords(Blog request)
        {
            var url = $"{ApiBaseURL}{APIs.CompetitorArticlePrimaryKeywords}";
            return await GetResponse(url);
        }

        public async Task<string?> AiGenerateSecondaryKeywords(Blog request)
        {
            var url = $"{ApiBaseURL}{APIs.CompetitorArticleSecondaryKeywords}";
            return await GetResponse(url);
        }

        public async Task<string?> AiGenerateContent(Blog request)
        {
            var url = $"{ApiBaseURL}{APIs.CompetitorArticleContent}";
            return await GetResponse(url);
        }

        public async Task<string?> AiGenerateMetaTags(Blog request)
        {
            var url = $"{ApiBaseURL}{APIs.CompetitorArticleMetaTags}";
            return await GetResponse(url);
        }
        private async Task<string?> GetResponse(string url)
        {
            try
            {
                var response = await httpClient.PostAsync(url, null);

                string? result = string.Empty;
                if (response != null && response.IsSuccessStatusCode)
                {
                    var mainResponse = JsonConvert.DeserializeObject<MainResponse>(await response.Content.ReadAsStringAsync());
                    if (mainResponse != null && mainResponse.IsSuccess)
                    {
                        result = JsonConvert.DeserializeObject<string?>(mainResponse.Content.ToString());
                    }
                }
                return result;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            // If we got this far, something failed, redisplay form
            return null;
        }
    }
}
