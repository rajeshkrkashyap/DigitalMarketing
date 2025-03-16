using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;

namespace ConnectToAi.Services
{
    public class CrawledService : BaseService
    {
        public CrawledService(ConfigService configService) : base(configService)
        {
        }

        public async Task<List<Crawled>> ListAsync(string projectId)
        {
            var returnResponse = new List<Crawled>();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectCrawledList}/?projectId=" + projectId;

                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<List<Crawled>>(contentStr);
                }
            }
            return returnResponse;
        }

    }
}
