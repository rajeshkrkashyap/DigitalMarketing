using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Core.Api.Services
{
    public class BlobStorageService
    {
        public string BlobConnectionString { get; set; }
        public string BlobContainer { get; set; }
        public async Task<string> GetImageAsDataUrlAsync(string imageName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(BlobConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(BlobContainer);
            BlobClient blobClient = containerClient.GetBlobClient(imageName);

            if (await blobClient.ExistsAsync())
            {
                BlobDownloadResult downloadInfo = blobClient.DownloadContent();
                byte[] imageBytes = downloadInfo.Content.ToMemory().ToArray(); // new byte[downloadInfo.ContentLength];
                //await downloadInfo.Content.rea(imageBytes, 0, (int)downloadInfo.Content.ToMemory().Length);
                string base64Image = Convert.ToBase64String(imageBytes);
                return $"data:{downloadInfo.Details.BlobType};base64,{base64Image}";
            }

            return null;
        }
        public string UploadImages(IFormFile file, string fileName)
        {

            var container = new BlobContainerClient(BlobConnectionString, BlobContainer);
            try
            {
                container.UploadBlobAsync("/" + fileName, file.OpenReadStream());
                return fileName;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public string UploadImages(Stream stream, string fileName)
        {

            var container = new BlobContainerClient(BlobConnectionString, BlobContainer);
            try
            {
                container.UploadBlobAsync("/" + fileName, stream);
                return fileName;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public void RawFileUpload(IFormFile file)
        {
            var container = new BlobContainerClient(BlobConnectionString, BlobContainer);
            try
            {
                container.UploadBlobAsync("RawFiles/" + file.Name, file.OpenReadStream());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void JsonLFileUpload(IFormFile file)
        {
            var container = new BlobContainerClient(BlobConnectionString, BlobContainer);
            container.UploadBlobAsync("JsonLFiles", file.OpenReadStream());
        }
        public string UploadImages(IBrowserFile file)
        {

            var container = new BlobContainerClient(BlobConnectionString, BlobContainer);
            try
            {
                FileInfo fileInfo = new FileInfo(file.Name);
                string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                container.UploadBlobAsync("/" + fileName, file.OpenReadStream(file.Size));
                return fileName;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public void RawFileUpload(IBrowserFile file)
        {
            var container = new BlobContainerClient(BlobConnectionString, BlobContainer);
            try
            {
                container.UploadBlobAsync("RawFiles/" + file.Name, file.OpenReadStream(file.Size));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void JsonLFileUpload(IBrowserFile file)
        {
            var container = new BlobContainerClient(BlobConnectionString, BlobContainer);
            container.UploadBlobAsync("JsonLFiles", file.OpenReadStream(file.Size));
        }
        public void Delete(string fileName)
        {
            var container = new BlobContainerClient(BlobConnectionString, BlobContainer);
        }
        public async Task<List<string>> Download(string fileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(BlobConnectionString);
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(BlobContainer);
            BlobClient blobClient = container.GetBlobClient(fileName);
            List<string> allLines = new List<string>();
            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadAsync();
                using (var streamReader = new StreamReader(response.Value.Content))
                {
                    while (!streamReader.EndOfStream)
                    {
                        allLines.Add(await streamReader.ReadLineAsync());
                        //Console.WriteLine(line);
                    }
                }
            }
            return allLines;
        }
    }
}
