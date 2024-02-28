using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using Core.Shared;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]

    public class OcrController : ControllerBase
    {
        [HttpPost("ReadImageData")]
        public async Task<string> ReadImageData(string imageUrl)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var requestUrl = "https://api.mathpix.com/v3/text";
                    MathPixRequest mathPixRequest = new MathPixRequest
                    {
                        src = imageUrl,
                        //src = "https://blobconnect.blob.core.windows.net/images/e502ee26-7c8d-49e8-ba57-045a59d90544.png",
                        formats = new string[] { "text", "data" },
                    };

                    mathPixRequest.data_options.include_asciimath = true;

                    var jsonData1 = JsonConvert.SerializeObject(mathPixRequest);
                    var requestBody = new StringContent(jsonData1, Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("app_id", "royallvastramm_3021fa_69d6d8");
                    httpClient.DefaultRequestHeaders.Add("app_key", "c3d738a0fe375a2cd6fa355e14b6c9ec2d33ea1a961f693d92a34f4fc970ebac");

                    var response = httpClient.PostAsync(requestUrl, requestBody).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                        //Console.WriteLine("Response: " + responseBody);
                    }

                    return response.StatusCode.ToString();
                }
                catch (Exception ex)
                {
                }
                return null;
            }
        }
    }
}
