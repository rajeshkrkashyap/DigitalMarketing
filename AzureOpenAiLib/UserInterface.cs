using Azure;
using Azure.AI.OpenAI;
using Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureOpenAiLib
{
    // Presentation Layer
    public class UserInterface
    {
        private readonly ITextGenerationService _textGenerationService;

        public UserInterface(ITextGenerationService textGenerationService)
        {
            _textGenerationService = textGenerationService;
        }

        public Response<ChatCompletions> GenerateText(AzurePromptInput azurePromptInput)
        {
            return _textGenerationService.GenerateText(azurePromptInput);
        }
        public async Task<Response<ChatCompletions>> GenerateTextAsync(AzurePromptInput azurePromptInput)
        {
            return await _textGenerationService.GenerateTextAsync(azurePromptInput);
        }
    }
}
