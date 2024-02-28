using Microsoft.AspNetCore.WebUtilities;
using Core.Shared;
using Newtonsoft.Json;
using System.Text;

namespace ConnectToAi.Services
{
    public class AuthService : BaseService
    {
        public AuthService(ConfigService configService) : base(configService)
        {
        }

        public async Task<HttpResponseMessage> SendSMS(string mobileNumber, string countryCode, string message)
        {
            if (!string.IsNullOrEmpty(mobileNumber) && !string.IsNullOrEmpty(countryCode) && !string.IsNullOrEmpty(message))
            {
                var url = $"{ApiBaseURL}{APIs.SMSSend}";
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        return await httpClient.GetAsync(url + "/?mobileNumber=" + mobileNumber + "&countryCode=" + countryCode + "&message=" + message);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            // If we got this far, something failed, redisplay form
            return null;
        }

        public async Task<HttpResponseMessage> MobileLoginAsync(LoginRegisterMobileViewModel loginViewModel)
        {
            if (loginViewModel != null)
            {
                var jsonData = JsonConvert.SerializeObject(loginViewModel);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var url = $"{ApiBaseURL}{APIs.MobileLogin}";
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        try
                        {
                            return await httpClient.PostAsync(url, content);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            // If we got this far, something failed, redisplay form
            return null;
        }
        public async Task<HttpResponseMessage> RegisterAsync(RegisterViewModel signUpViewModel)
        {
            if (signUpViewModel != null)
            {
                signUpViewModel.EmailConfirmUrl = $"{ApiBaseURL}/confirmemail";

                var jsonData = JsonConvert.SerializeObject(signUpViewModel);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var url = $"{ApiBaseURL}{APIs.Register}";
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        return await httpClient.PostAsync(url, content);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            // If we got this far, something failed, redisplay form
            return null;
        }
        public async Task<HttpResponseMessage> LoginAsync(LoginViewModel loginViewModel)
        {
            if (loginViewModel != null)
            {
                var jsonData = JsonConvert.SerializeObject(loginViewModel);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var url = $"{ApiBaseURL}{APIs.Login}";
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        return await httpClient.PostAsync(url, content);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            // If we got this far, something failed, redisplay form
            return null;
        }
        public async Task<HttpResponseMessage> ConfirmEmail(Uri uri)
        {
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("userid", out var userId);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("token", out var token);

            var url = $"{ApiBaseURL}{APIs.ConfirmEmail}";
            using (HttpClient httpClient = new HttpClient())
            {
                return await httpClient.GetAsync(url + "/?userId=" + userId + "&token=" + token);
            }
        }
    }
}
