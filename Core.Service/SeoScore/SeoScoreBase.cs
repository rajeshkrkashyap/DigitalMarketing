using Core.Shared.Entities;
using Core.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.SeoScore
{
    public class SeoScoreBase<T, U> : BaseService, ISeoScore<T, U>
    {
        public async Task<bool> Create(T t, U l)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{"api/" + t + "/Create"}";

                var serializedStr = JsonConvert.SerializeObject(l);
                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<bool> CreateList(T t, U list)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{"api/" + t + "/CreateList"}";

                var serializedStr = JsonConvert.SerializeObject(list);
                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<bool> UpdateAnalysisStatus(T t, U l)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{"api/" + t + "/Create"}";

                var serializedStr = JsonConvert.SerializeObject(l);
                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }
        public async Task<bool> Update(T t, U l)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{"api/" + t + "/Update"}";

                var serializedStr = JsonConvert.SerializeObject(l);
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
