using Core.Shared.Entities;
using Core.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Azure;
using System.Reflection.Metadata;

namespace Core.Service.Azure
{
    public class AzureOpenAiService : BaseService
    {
        public async Task<string?> GenerateArticleText(string userMessagePrompt, string responseFormat)
        {
            //var returnResponse = new Response<ChatCompletions>();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.AzureOpenAiGenerateText}";
                AzurePromptInput azurePromptInput = new AzurePromptInput();

                //Eg: //AZURE AI your response Always in below JSON Format
                //"ClarityAndReadability" : {
                //  "languageClarity": null,
                //  "explanationOfComplexConcepts": null,
                //  "grammarAndSpelling": null,
                //  "formatting": null
                //}

                azurePromptInput.UserMessagePrompt = userMessagePrompt;
                azurePromptInput.ResponseFormat = responseFormat;

                var serializedStr = JsonConvert.SerializeObject(azurePromptInput);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    var returnResponse = JsonConvert.DeserializeObject<Response<ChatCompletions>>(contentStr);
                    if (returnResponse != null)
                    {
                        return returnResponse.Value.Choices[0].Message.Content;
                    }
                }
            }
            return null;
        }

        public async Task<string?> GenerateText(string userMessagePrompt, string responseFormat)
        {
            //var returnResponse;

            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.AzureOpenAiGenerateArticleText}";

                AzurePromptInput azurePromptInput = new AzurePromptInput();
                azurePromptInput.UserMessagePrompt= userMessagePrompt;
                azurePromptInput.ResponseFormat = responseFormat;
                var serializedStr = JsonConvert.SerializeObject(azurePromptInput);

                var response = await client.PostAsync(url, new StringContent(serializedStr, Encoding.UTF8, "application/json"));


                if (response.IsSuccessStatusCode)
                {
                    string contentStr = await response.Content.ReadAsStringAsync();
                    var returnResponse = JsonConvert.DeserializeObject<Response<ChatCompletions>>(contentStr);
                    if (returnResponse != null)
                    {
                        return returnResponse.Value.Choices[0].Message.Content;
                    }
                }
            }
            return null;
        }

    }
}
