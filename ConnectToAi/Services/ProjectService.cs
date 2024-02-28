using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System.Text;

namespace ConnectToAi.Services
{
    public class ProjectService : BaseService
    {
        public ProjectService(ConfigService configService) : base(configService)
        {
        }

        public async Task<List<Project>> ListAsync(string userId)
        {
            var returnResponse = new List<Project>();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectList}/?userId=" + userId;

                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<List<Project>>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<Project> GetById(string id)
        {
            var returnResponse = new Project();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectGetById}/?id=" + id;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<Project>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<Project> GetByTitle(string name, string appUserId)
        {
            var returnResponse = new Project();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectGetByName}/?name=" + name + "&appUserId=" + appUserId;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<Project>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> IsTitleExist(string name, string appUserId)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectIsExist}/?name=" + name + "&appUserId=" + appUserId;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<Project> CreateAsync(Project Project)
        {
            var returnResponse = new Project();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectCreate}";

                var serializedStr = JsonConvert.SerializeObject(Project);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var returnResponseStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<Project>(returnResponseStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> UpdateParent(string id, string ParentId)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectUpdate}/?id=" + id + "&ParentId=" + ParentId;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<Project> Update(string id)
        {
            var returnResponse = new Project();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectUpdate}/?id=" + id;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<Project>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<bool> Delete(string id)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectDelete}/?id=" + id;
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
