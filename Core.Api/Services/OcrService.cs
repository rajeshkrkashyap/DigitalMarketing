using Microsoft.AspNetCore.Http;
using Core.Shared;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;

namespace Core.Api.Services
{
    public class OcrService
    {
        public async Task<string> ReadImageData(string fileName, Stream stream, bool isStorageAzure)
        {
            string imageUrl = "";
            if (isStorageAzure)
            {
                imageUrl = "https://blobconnect.blob.core.windows.net/images/" + fileName;
            }
            else
            {
                UploadFileToFtp(stream, fileName);
                imageUrl = "https://connectto.ai/propmt-image/" + fileName;
            }
            return await ProcessImage(imageUrl);
        }
        public async Task<string> ReadImageData(string fileName, IFormFile file, bool isStorageAzure)
        {
            try
            {
                string imageUrl = "";
                if (isStorageAzure)
                {
                    imageUrl = "https://blobconnect.blob.core.windows.net/filecontainer/" + fileName;
                }
                else
                {
                    UploadFileToFtp(file, fileName);
                    imageUrl = "https://connectto.ai/propmt-image/" + fileName;
                }

                return await ProcessImage(imageUrl);
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        
        #region Private code functions
        private static void UploadFileToFtp(IFormFile file, string fileName)
        {
            byte[] buffer;
            using (Stream fs = file.OpenReadStream())
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
            }
            UploadToFTP(fileName, buffer);
        }
        private static void UploadFileToFtp(Stream stream, string fileName)
        {
            byte[] buffer;
            buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            UploadToFTP(fileName, buffer);
        }
        private static void UploadToFTP(string fileName, byte[] buffer)
        {
            try
            {
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
        private async Task<string> ProcessImage(string imageUrl)
        {
            var requestUrl = "https://api.mathpix.com/v3/text";
            MathPixRequest mathPixRequest = new MathPixRequest
            {
                src = imageUrl,
                formats = new string[] { "text", "data" },
            };

            mathPixRequest.data_options.include_asciimath = true;
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var jsonData1 = JsonConvert.SerializeObject(mathPixRequest);
                var requestBody = new StringContent(jsonData1, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Add("app_id", "royallvastramm_3021fa_69d6d8");
                httpClient.DefaultRequestHeaders.Add("app_key", "c3d738a0fe375a2cd6fa355e14b6c9ec2d33ea1a961f693d92a34f4fc970ebac");

                var response = httpClient.PostAsync(requestUrl, requestBody).Result;

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            return null;
        }
        #endregion
    }
}
