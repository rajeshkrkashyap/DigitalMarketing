using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System.Text;

namespace ConnectToAi.Services
{
    public class BlogService : BaseService
    {
        readonly HttpClient httpClient; 
        public BlogService(ConfigService configService) : base(configService)
        {
            httpClient = new HttpClient();
        }
        public async Task<IEnumerable<Blog>> BlogList()
        {
            // In a real application, send the OTP via SMS service
            var url = $"{ApiBaseURL}{APIs.BlogList}";
            try
            {
                //using (HttpClient httpClient = new HttpClient())
                //{
                var response = await httpClient.PostAsync(url, null);

                List<Blog>? blogList = null;
                if (response != null && response.IsSuccessStatusCode)
                {
                    var mainResponse = JsonConvert.DeserializeObject<MainResponse>(await response.Content.ReadAsStringAsync());
                    if (mainResponse != null && mainResponse.IsSuccess)
                    {
                        blogList = JsonConvert.DeserializeObject<List<Blog>>(mainResponse.Content.ToString());
                    }
                }
                return blogList;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            // If we got this far, something failed, redisplay form
            return null;
        }
        public async Task<Blog> BlogGetById(string id)
        {
            // In a real application, send the OTP via SMS service
            var url = $"{ApiBaseURL}{APIs.BlogGetById}";
            try
            {
                //using (HttpClient httpClient = new HttpClient())
                //{
                var response = await httpClient.PostAsync(url + "/?id=" + id, null);
                Blog? Blog = null;
                if (response != null && response.IsSuccessStatusCode)
                {
                    var mainResponse = JsonConvert.DeserializeObject<MainResponse>(await response.Content.ReadAsStringAsync());
                    if (mainResponse != null && mainResponse.IsSuccess)
                    {
                        Blog = JsonConvert.DeserializeObject<Blog>(mainResponse.Content.ToString());
                    }
                }
                return Blog;

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            // If we got this far, something failed, redisplay form
            return null;
        }
        public async Task<bool> Create(Blog Blog)
        {
            var url = $"{ApiBaseURL}{APIs.BlogCreate}";
            try
            {
                //using (HttpClient httpClient = new HttpClient())
                //{
                //Serialize the testResult to JSON
                string jsonContent = JsonConvert.SerializeObject(Blog);
                //Create the HttpContent with the JSON and specify the content type as "application/json"
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                //Send the POST request with the serialized content
                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
                //Check if the response was successful
                return response.IsSuccessStatusCode;
                //}
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            //something failed
            return false;
        }
        public async Task<bool> Update(Blog Blog)
        {
            var url = $"{ApiBaseURL}{APIs.BlogUpdate}";
            try
            {
                //using (HttpClient httpClient = new HttpClient())
                //{
                //Serialize the testResult to JSON
                string jsonContent = JsonConvert.SerializeObject(Blog);
                //Create the HttpContent with the JSON and specify the content type as "application/json"
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                //Send the POST request with the serialized content
                HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
                //Check if the response was successful
                return response.IsSuccessStatusCode;
                //}
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            //something failed
            return false;
        }
    }
}
