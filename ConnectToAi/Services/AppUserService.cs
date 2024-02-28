using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System.Text;

namespace ConnectToAi.Services
{
    public class AppUserService : BaseService
    {
        public AppUserService(ConfigService configService) : base(configService)
        {
        }
        public async Task<List<AppUser>> ListAsync()
        {
            var returnResponse = new List<AppUser>();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.AppUserList}";

                var serializedStr = JsonConvert.SerializeObject("");

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<List<AppUser>>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<AppUser> GetById(string id)
        {
            var returnResponse = new AppUser();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.AppUserGetById}/?id=" + id;
                var response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<AppUser>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<AppUser> UpdateToken(AppUserViewModel appUserViewModel)
        {
            var returnResponse = new AppUser();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.AppUserUpdateTokens}";
                var serializedStr = JsonConvert.SerializeObject(appUserViewModel);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<AppUser>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<AppUser> Update(AppUser appUser)
        {
            var returnResponse = new AppUser();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.AppUserUpdate}";
                var serializedStr = JsonConvert.SerializeObject(appUser);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<AppUser>(contentStr);
                }
            }
            return returnResponse;
        }

    }
}
