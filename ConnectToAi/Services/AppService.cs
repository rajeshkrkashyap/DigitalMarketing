using Core.Shared;
using Newtonsoft.Json;
using System.Text;

namespace ConnectToAi.Services
{
    public class AppService : BaseService, IAppService
    {
        public AppService(ConfigService configService) : base(configService)
        {
        }

        public async Task<AuthUserResponse> AuthenticateUser(LoginViewModel loginModel, string apiBaseUrl)
        {
            var returnResponse = new AuthUserResponse();
            using (var client = new HttpClient())
            {
                var url = $"{apiBaseUrl}{APIs.Login}";

                var serializedStr = JsonConvert.SerializeObject(loginModel);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<AuthUserResponse>(contentStr);
                }
            }
            return returnResponse;
        }

        public async Task<string> RefreshToken(UserDetail userDetail)
        {
 
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.RefreshToken}";

                var serializedStr = JsonConvert.SerializeObject(new AuthenticationResponse
                {
                    RefreshToken = userDetail.RefreshToken,
                    AccessToken = userDetail.AccessToken
                });

                try
                {
                    var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string contentStr = await response.Content.ReadAsStringAsync();
                        var userManagerResponse = JsonConvert.DeserializeObject<MainResponse>(contentStr);
                        if (userManagerResponse != null && userManagerResponse.IsSuccess)
                        {
                            var tokenDetails = JsonConvert.DeserializeObject<AuthenticationResponse>(userManagerResponse.Content.ToString());

                            userDetail.AccessToken = tokenDetails.AccessToken;
                            userDetail.RefreshToken = tokenDetails.RefreshToken;
                            JsonSerializerSettings settings = new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            };


                            return JsonConvert.SerializeObject(userDetail, settings);
                            //await SecureStorage.SetAsync(nameof(Setting.UserDetail), userDetailsStr);
                            //isTokenRefreshed = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
            return null;
        }

        //public async Task<List<StudentModel>> GetAllStudents()
        //{
        //    var returnResponse = new List<StudentModel>();
        //    using (var client = new HttpClient())
        //    {
        //        var url = $"{Setting.BaseUrl}{APIs.GetAllStudents}";

        //        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Setting.UserDetail?.AccessToken}");
        //        var response = await client.GetAsync(url);

        //        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //        {
        //            bool isTokenRefreshed = await RefreshToken();
        //            if (isTokenRefreshed) return await GetAllStudents();
        //        }
        //        else
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string contentStr = await response.Content.ReadAsStringAsync();
        //                var UserManagerResponse = JsonConvert.DeserializeObject<UserManagerResponse>(contentStr);
        //                if (UserManagerResponse.IsSuccess)
        //                {
        //                    returnResponse = JsonConvert.DeserializeObject<List<StudentModel>>(UserManagerResponse.Content.ToString());
        //                }
        //            }
        //        }

        //    }
        //    return returnResponse;
        //}


        //public async Task<(bool IsSuccess, string ErrorMessage)> RegisterUser(RegisterViewModel registerUser, string apiBaseUrl)
        //{
        //    string errorMessage = string.Empty;
        //    bool isSuccess = false;
        //    using (var client = new HttpClient())
        //    {
        //        var url = $"{apiBaseUrl}{APIs.Register}";

        //        var serializedStr = JsonConvert.SerializeObject(registerUser);
        //        var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));
        //        if (response.IsSuccessStatusCode)
        //        {
        //            isSuccess = true;
        //        }
        //        else
        //        {
        //            errorMessage = await response.Content.ReadAsStringAsync();
        //        }
        //    }
        //    return (isSuccess, errorMessage);
        //}
    }
}
