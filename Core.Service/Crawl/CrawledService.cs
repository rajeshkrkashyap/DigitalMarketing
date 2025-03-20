using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Crawl
{
    public class CrawledService : BaseService
    {
        public async Task<Crawled> GetPageContent(string projectId, string pageUrl)
        {
            var returnResponse = new Crawled();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.CrawledPageContent}/?projectId=" + projectId + "&url=" + pageUrl; 
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<Crawled>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<bool> Create(Crawled crawled)
        {
            var returnResponse =false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.CrawledCreate}";

                var serializedStr = JsonConvert.SerializeObject(crawled);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<bool> UpdateAnalysisStatus(Crawled crawled)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.CrawledUpdateAnalysisStatus}";

                var serializedStr = JsonConvert.SerializeObject(crawled);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<bool> Update(Crawled crawled)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.CrawledUpdate}";

                var serializedStr = JsonConvert.SerializeObject(crawled);

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
