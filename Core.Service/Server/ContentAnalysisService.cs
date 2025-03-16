
using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Server
{
    public class ContentAnalysisService : BaseService
    {
        public async Task<bool> Create(ContentAnalysis contentAnalysis)
        {
            var returnResponse =false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ContentAnalysisCreate}";

                var serializedStr = JsonConvert.SerializeObject(contentAnalysis);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> UpdateMetaTagKeywords(ContentAnalysis contentAnalysis)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ContentAnalysisUpdateMetaTagKeywords}";

                var serializedStr = JsonConvert.SerializeObject(contentAnalysis);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> UpdateHeadings(ContentAnalysis contentAnalysis)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ContentAnalysisUpdateHeadings}";

                var serializedStr = JsonConvert.SerializeObject(contentAnalysis);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> UpdateKeywordFrequency(ContentAnalysis contentAnalysis)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ContentAnalysisUpdateKeywordFrequency}";

                var serializedStr = JsonConvert.SerializeObject(contentAnalysis);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> UpdateMetaDescription(ContentAnalysis contentAnalysis)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ContentAnalysisUpdateMetaDescription}";

                var serializedStr = JsonConvert.SerializeObject(contentAnalysis);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> UpdateTitle(ContentAnalysis contentAnalysis)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ContentAnalysisUpdateTitle}";

                var serializedStr = JsonConvert.SerializeObject(contentAnalysis);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }
    }
}
