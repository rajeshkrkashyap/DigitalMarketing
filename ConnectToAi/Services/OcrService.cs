using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using System.Net;
using System.Text;
using static System.Net.WebRequestMethods;

namespace ConnectToAi.Services
{
    public class OcrService : BaseService
    {
        public OcrService(ConfigService configService) : base(configService)
        {
        }

        private void UploadImage(string path)
        {
            throw new NotImplementedException();
        }
        static void UploadFileToFtp(IFormFile file, string fileName)
        {
            try
            {
                byte[] buffer;
                using (Stream fs = file.OpenReadStream())
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                }

                string ftpServer = "ftp://win5045.site4now.net/";
                string userName = "propmtuser";
                string password = "Mokshit@123";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(userName, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
        public async Task<string> ReadImageData(string uploadPath, string host, string fileName, IFormFile file, bool isStorageAzure)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {

                    string imageUrl = "";
                    if (isStorageAzure)
                    {
                        imageUrl = "https://blobconnect.blob.core.windows.net/images/" + fileName;
                    }
                    else
                    {
                        UploadFileToFtp(file, fileName);
                        imageUrl = "https://connectto.ai/propmt-image/" + fileName;
                    }

                    var returnResponse = "";
                    using (var client = new HttpClient())
                    {
                        var url = $"{ApiBaseURL}{APIs.OCRReadImageData}";

                        var serializedStr = JsonConvert.SerializeObject(imageUrl);

                        var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                        if (response.IsSuccessStatusCode)
                        {
                            string contentStr = await response.Content.ReadAsStringAsync();
                            returnResponse = JsonConvert.DeserializeObject<string>(contentStr);
                        }
                    }
                    return returnResponse;
                }
                catch (Exception ex)
                {
                }
                return null;
            }
        }
    }
}