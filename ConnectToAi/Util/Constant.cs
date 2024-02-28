using System.Security.Cryptography;
using System.Text;

namespace ConnectToAi.Util
{
    public class Constant
    {
        public const string PromptModel_Text_Davinci_003 = "text-davinci-003";
        public const string PromptModel_GPT_3_5_Turbo = "gpt-3.5-turbo";
        public const string OutputFolderPath = "c:/files/JSONL_Files";
        public const string InputFolderPath = "c:/files/uploads";
        public const string ChatCompletionsApi = "https://api.openai.com/v1/chat/completions";
        public const string FineTuneApi = "https://api.openai.com/v1/files";
        public const string BlobConnectionString = "DefaultEndpointsProtocol=https;AccountName=blobconnect;AccountKey=n9Si1znvSFjOPw1xioHiPq/8w28Cy4CBFKvPKtABOKdyJB7dF7ubZz6wFPiB9zLGmZ5kac4NK/lL+AStGKwUlA==;EndpointSuffix=core.windows.net";
        public const string BlobFilesContainer = "files";
        public const string BlobImagesContainer = "images";
    }
    public static class Utility
    {
        public static string Encrypt(string plainText, string key)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Use a proper initialization vector (IV) in production

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string key)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Use the same IV that was used for encryption

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
