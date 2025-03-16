using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using Azure.Core.Shared;
using Core.Shared;
namespace AzureOpenAiLib
{
    
    // Application Layer
    public interface  ITextGenerationService
    {
        Response<ChatCompletions> GenerateText(AzurePromptInput azurePromptInput);
        Task<Response<ChatCompletions>> GenerateTextAsync(AzurePromptInput azurePromptInput);
    }

    
}
