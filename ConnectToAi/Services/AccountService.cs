using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConnectToAi.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetDetailAsync(string authToken);
    }
    public class AccountService : IAccountService
    {
        readonly HttpClient _httpClient;
        readonly ConfigService _dataService;
        public AccountService(HttpClient httpClient, ConfigService dataService)
        {
            _httpClient = httpClient;
            _dataService = dataService;
        }

        public async Task<IEnumerable<Account>> GetDetailAsync(string authToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            var url = _dataService.AppSettings.ApiBaseUrl + "api/Account/GetDetail";
            var response = await _httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<IEnumerable<Account>>(responseBody);

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                //return message "UnAuthorized Access";
            }
            if (account == null)
            {
                return new List<Account>();// return empty list
            }
            return account;
        }
    }
}
