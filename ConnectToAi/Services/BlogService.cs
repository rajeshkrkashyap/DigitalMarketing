using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System.Text;

namespace ConnectToAi.Services
{
    public class BlogService : BaseService
    {
        public BlogService(ConfigService configService) : base(configService)
        {
        }

        public async Task<List<Blog>> ListAsync()
        {
            var returnResponse = new List<Blog>();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.BlogList}";

                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<List<Blog>>(contentStr);
                }
            }
            return returnResponse;
        }

        

        public async Task<Blog> GetById(string id)
        {
            var returnResponse = new Blog();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.BlogGetById}/?id=" + id;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<Blog>(contentStr);
                }
            }
            return returnResponse;
        }

       
        public async Task<bool> IsTitleExist(string title)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.BlogIsExist}/?title=" + title;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<Blog> CreateAsync(Blog Blog)
        {
            var returnResponse = new Blog();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.BlogCreate}";

                var serializedStr = JsonConvert.SerializeObject(Blog);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var returnResponseStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<Blog>(returnResponseStr);
                }
            }
            return returnResponse;
        }

   

        public async Task<Blog> Update(string id)
        {
            var returnResponse = new Blog();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.BlogUpdate}/?id=" + id;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<Blog>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> Delete(string id)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.BlogDelete}/?id=" + id;
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
