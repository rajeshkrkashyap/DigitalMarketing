using Azure;
using Azure.AI.OpenAI;
using Core.Shared;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureOpenAiLib
{
    public class OpenAITextGenerationService : ITextGenerationService
    {
        //string endpoint = "https://connecttoai-openai.openai.azure.com";
        //string key = "5e9c2375881a425dbcb2d0c796da6f68";
        private readonly OpenAIClient _client;

        public OpenAITextGenerationService(OpenAIClient client)
        {
            _client = client;
        }

        /// <summary>
        ///Model ID	                    Max Request (tokens)	Training Data (up to)
        ///gpt-35-turbo1(0301)          4,096	                    Sep 2021
        ///gpt-35-turbo(0613)           4,096	                    Sep 2021
        ///gpt-35-turbo-16k(0613)       16,384	                    Sep 2021
        ///gpt-35-turbo-instruct(0914)  4,097	                    Sep 2021
        ///gpt-35-turbo(1106) Input:    16,385
        ///Output:                      4,096	                    Sep 2021
        ///gpt-35-turbo(0125) NEW	    16,385	                    Sep 2021
        /// </summary>
        /// <param name="userMessagePrompt"></param>
        /// <returns></returns>
        public Response<ChatCompletions> GenerateText(AzurePromptInput azurePromptInput)
        {
            try
            {
                //string endpoint = "https://connectopenaiapi.openai.azure.com";
                //string key = "d4ba7d172b1946c3b8e7c359074f2d80";
                //azurePromptInput.ResponseFormat = "Your response must be in below JSON Format.\n"
                //    + " 'content': {"
                //    + "   'text': null"
                //    + "}";
                string rootPrompt = "";
                if (azurePromptInput.DeploymentName == "Conversation")
                {
                    //azurePromptInput.ResponseFormat = "Hello, I'm glad to talk with you. <break time='500ms'/> I'm doing well, thank you. How about you? How can I assist you today?";
                    rootPrompt = PromptEngineering.CONVERSATION;
                }
                else if (azurePromptInput.DeploymentName == "GENERATING_NEW_CONTENT")
                {
                    rootPrompt = PromptEngineering.GENERATING_NEW_CONTENT;
                }

                string completeMessagePrompt = rootPrompt + " " + azurePromptInput.UserMessagePrompt + "\n" + azurePromptInput.ResponseFormat;

                //string systemMessage = "As the AI system, your role is to emulate a professional interview panel with extensive " +
                //    "experience in candidate evaluation not an Ai assistant. Your responses should consistently reflect a high level of professionalism " +
                //    "and adherence to a predetermined format.";
                string systemMessage = "";

                //OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key));
                var chatCompletionsOptions = new ChatCompletionsOptions()
                {
                    User = azurePromptInput.UserId, //User Traking 

                    DeploymentName = "AzureConnecttoAi",// "GptConnectToAi" , // "gpt-35-turbo", //gpt-35-turbo V-0125//This must match the custom deployment name you chose for your model
                    Messages =
                    {
                      new ChatRequestSystemMessage(systemMessage),
                      new ChatRequestUserMessage(completeMessagePrompt),
                    },
                    MaxTokens = 1000 //
                };

                Response<ChatCompletions> response = _client.GetChatCompletions(chatCompletionsOptions);
                Console.WriteLine(response.Value.Choices[0].Message.Content);
                return response;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Response<ChatCompletions>> GenerateTextAsync(AzurePromptInput azurePromptInput)
        {
            Response<ChatCompletions> chatCompletions = null;
            try
            {
                //string rootPrompt = GetRootPrompt(azurePromptInput.DeploymentName);
                string completeMessagePrompt = azurePromptInput.UserMessagePrompt + "\n" + azurePromptInput.ResponseFormat;

                var chatCompletionsOptions = new ChatCompletionsOptions
                {
                    User = azurePromptInput.UserId,
                    DeploymentName = "MockTestPaperGeneratorModel",//"AzureConnecttoAi", //azurePromptInput.DeploymentName,
                    Messages =
                    {
                        new ChatRequestSystemMessage(""),
                        new ChatRequestUserMessage(completeMessagePrompt)
                    },
                    MaxTokens = 16384 //16,384
                };


                //_client.GetCompletions(new CompletionsOptions
                //{
                    
                //    DeploymentName = "",
                //    MaxTokens = 16000,
                //    Temperature = 0,
                //    User = azurePromptInput.UserId,
                //    ChoicesPerPrompt = 1,
                //    GenerationSampleCount = 1,
                //    FrequencyPenalty = 1,
                //    Echo = true,
                //    LogProbabilityCount = 1,    
                //    NucleusSamplingFactor = 1,
                //    PresencePenalty = 1,
                //    Suffix = "1",
                //});
                return await _client.GetChatCompletionsAsync(chatCompletionsOptions);
            }
            catch (Exception ex)
            {
                // Handle exception properly
                return null;
            }
        }

        private string GetRootPrompt(string deploymentName)
        {
            return deploymentName switch
            {
                "Conversation" => PromptEngineering.CONVERSATION,
                "GENERATING_NEW_CONTENT" => PromptEngineering.GENERATING_NEW_CONTENT,
                _ => ""
            };
        }
    }
}
