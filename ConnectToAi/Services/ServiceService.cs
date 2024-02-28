using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System.Text;

namespace ConnectToAi.Services
{
    public class ServiceService :BaseService
    {
        public ServiceService(ConfigService configService) : base(configService)
        {
        }

        public async Task<List<ApplicationService>> ListAsync()
        {
            var returnResponse = new List<ApplicationService>();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ServiceList}";

                var serializedStr = JsonConvert.SerializeObject("");

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<List<ApplicationService>>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<ApplicationService> GetById(string id)
        {
            var returnResponse = new ApplicationService();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ServiceGetById}/?id=" + id;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<ApplicationService>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<ApplicationService> GetByName(string name)
        {
            var returnResponse = new ApplicationService();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ServiceGetByName}/?name=" + name;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<ApplicationService>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<ApplicationService> CreateAsync(ApplicationService Service)
        {
            var returnResponse = new ApplicationService();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ServiceCreate}";

                var serializedStr = JsonConvert.SerializeObject(Service);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<ApplicationService>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<ApplicationService> Update(string id)
        {
            var returnResponse = new ApplicationService();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ServiceUpdate}/?id=" + id;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<ApplicationService>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> Delete(string id)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ServiceDelete}/?id=" + id;
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
