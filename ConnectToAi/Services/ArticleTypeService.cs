using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System.Text;

namespace ConnectToAi.Services
{
    public class ArticleTypeService : BaseService
    {
        public ArticleTypeService(ConfigService configService) : base(configService)
        {
        }

        public async Task<List<ArticleType>?> ListAsync(string projectId)
        {
            var returnResponse = new List<ArticleType>();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ArticleTypeList}/?projectId=" + projectId;

                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<List<ArticleType>>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<int> ArticleTypeCount(string projectId)
        {
            var returnResponse = 0;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ArticleTypeCount}/?projectId=" + projectId;

                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<int>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<ArticleType?> GetById(string id)
        {
            var returnResponse = new ArticleType();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ArticleTypeGetById}/?id=" + id;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<ArticleType>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<ArticleType?> CreateAsync(ArticleType ArticleType)
        {
            var returnResponse = new ArticleType();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ArticleTypeCreate}";

                var serializedStr = JsonConvert.SerializeObject(ArticleType);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var returnResponseStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<ArticleType>(returnResponseStr);
                }
            }
            return returnResponse;
        }

        public async Task<ArticleType?> Update(string id)
        {
            var returnResponse = new ArticleType();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ArticleTypeUpdate}/?id=" + id;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<ArticleType>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> Delete(string id)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ArticleTypeDelete}/?id=" + id;
                var response = await client.PostAsync(url, null);

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
