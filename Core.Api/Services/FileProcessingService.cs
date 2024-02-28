using Microsoft.AspNetCore.Http;
using Core.Shared;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;


namespace Core.Api.Services
{
    public class FileProcessingService
    {
        public async Task<string> ExtractFileContent(string base64String)
        {
            var fileExtention = DetermineFileExtension(base64String);
            if (fileExtention == "unknown")
            {
                return "ConnectToAi:Error";
            }
            byte[] byteArray = Convert.FromBase64String(base64String);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                OcrService _ocrService = new OcrService();
                var isAzureStorage = false;
                MathOCRResponse _imageContent = null;
                string fileName = Guid.NewGuid().ToString() + fileExtention; //file.FileName;

                if (!isAzureStorage)
                {
                    try
                    {
                        //var uploadPath = _hostingEnvironment.ContentRootPath;
                        var imageData = await _ocrService.ReadImageData(fileName, stream, false);
                        _imageContent = JsonConvert.DeserializeObject<MathOCRResponse>(imageData);
                        var _blobPath = "https://connectto.ai/propmt-image/" + fileName;
                        return  _imageContent.Text;
                        //return Json(new { success = true, blobPath = _blobPath, imageContent = _imageContent.Text});
                    }
                    catch (Exception ex)
                    {
                        return "ConnectToAi:Error";
                        //return Json(new { success = false, message = "Error uploading file: " + ex.Message });
                    }
                }
                else
                {
                    try
                    {
                        BlobStorageService _blobStorageService = new BlobStorageService();
                        _blobStorageService.BlobConnectionString = "DefaultEndpointsProtocol=https;AccountName=blobconnect;AccountKey=QjcFGkTcmbG4XpJNEBI2aQ2yDwytdhCWq/o2YuFnU9lsobS2vPZF/HdIbxaiDX1VaY4p+Q1Klur4+AStu5rpMA==;EndpointSuffix=core.windows.net";
                        _blobStorageService.BlobContainer = "filecontainer";//appSettings.BlobImagesContainer;
                        fileName = _blobStorageService.UploadImages(stream, fileName);

                        string _blobPath = "";

                        while (true)
                        {
                            if (string.IsNullOrEmpty(_blobPath))
                            {
                                _blobPath = await _blobStorageService.GetImageAsDataUrlAsync(fileName);
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (_blobPath != null)
                        {
                            var imageData = await _ocrService.ReadImageData(fileName, stream, true);
                            _imageContent = JsonConvert.DeserializeObject<MathOCRResponse>(imageData);
                        }
                        return _imageContent.Text;
                        //return Json(new { success = true, blobPath = _blobPath, imageContent = _imageContent.text });
                    }
                    catch (Exception ex)
                    {
                        return "ConnectToAi:Error";
                        //return Json(new { success = false, message = "Error uploading file: " + ex.Message });
                    }
                }
            }
        }

        public static IFormFile ConvertBase64ToFile(string base64String)
        {

            var fileExtention = DetermineFileExtension(base64String);

            var fileName = Guid.NewGuid().ToString() + fileExtention;

            // Decode base64 string to byte array

            byte[] fileBytes = Convert.FromBase64String(base64String);

            // Determine MIME type (you may need to use a library for more accurate detection)
            string mimeType = DetermineMimeType(fileName);

            // Create a MemoryStream from the byte array
            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            {
                // Create a FormFile object
                var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = mimeType
                };

                return formFile;
            }
        }
        public static string DetermineMimeType(string fileName)
        {
            // Here, you may use a library like MimeMapping.GetMimeMapping from Microsoft.AspNetCore.StaticFiles
            // Alternatively, you can analyze the file signature to determine the MIME type

            // For simplicity, let's use a basic example (you may need to enhance this for accurate detection)
            if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                return "image/png";
            }
            else if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return "image/jpeg";
            }
            else if (fileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            {
                return "image/gif";
            }
            else
            {
                // Default to application/octet-stream if MIME type cannot be determined
                return "application/octet-stream";
            }
        }
        public static string DetermineFileExtension(string base64String)
        {
            // Decode base64 string to byte array
            byte[] fileBytes = Convert.FromBase64String(base64String);

            // Analyze the first few bytes to determine the file extension
            string fileExtension = AnalyzeFileBytes(fileBytes);

            return fileExtension;
        }
        public static string AnalyzeFileBytes(byte[] fileBytes)
        {
            // Check for common file signatures
            if (StartsWith(fileBytes, "FFD8FFE0")) // JPEG
            {
                return ".jpg";
            }
            else if (StartsWith(fileBytes, "89504E47")) // PNG
            {
                return ".png";
            }
            else if (StartsWith(fileBytes, "47494638")) // GIF
            {
                return ".gif";
            }
            else if (StartsWith(fileBytes, "25504446")) // PDF
            {
                return ".pdf";
            }
            else if (StartsWith(fileBytes, "504B0304")) // .docx, .pptx, .xlsx (Office Open XML)
            {
                return ".docx";
            }
            else if (StartsWith(fileBytes, "D0CF11E0A1B11AE1")) // .doc, .xls (OLE Compound File)
            {
                return ".doc";
            }
            else if (StartsWith(fileBytes, "504B030414000600")) // .xlsx (Office Open XML)
            {
                return ".xlsx";
            }
            else if (StartsWith(fileBytes, "D0CF11E0A1B11AE1")) // .xls (OLE Compound File)
            {
                return ".xls";
            }
            else if (IsAsciiText(fileBytes)) // .txt (ASCII text)
            {
                return ".txt";
            }
            // Add more checks for other file types...

            // Default to unknown extension
            return "unknown";
        }
        public static bool IsAsciiText(byte[] fileBytes)
        {
            // Check if the content appears to be ASCII text
            foreach (var b in fileBytes)
            {
                if (b < 32 || b > 126)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool StartsWith(byte[] byteArray, string hexPrefix)
        {
            // Convert the hex prefix to a byte array
            byte[] prefixBytes = StringToByteArray(hexPrefix);

            // Check if the byteArray starts with the specified hex prefix
            return byteArray.Length >= prefixBytes.Length &&
                   byteArray.Take(prefixBytes.Length).SequenceEqual(prefixBytes);
        }
        public static byte[] StringToByteArray(string hex)
        {
            // Convert hex string to byte array
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
