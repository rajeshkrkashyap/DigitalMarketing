using Azure;
using Azure.AI.OpenAI;
using AzureOpenAiLib;
using Core.Api.Models;
using Core.Shared;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureOpenAiController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly OpenAIConfiguration _config;
        private readonly OpenAITextGenerationFactory _factory;
        private readonly OpenAIClient _client;
        public AzureOpenAiController(ApplicationDbContext dbContext, OpenAIClient client, OpenAIConfiguration config, OpenAITextGenerationFactory factory)
        {
            _dbContext = dbContext;
            _config = config;
            //_config.ApiKey = "5e9c2375881a425dbcb2d0c796da6f68";
            _factory = factory;
            _client = client;
        }

        [HttpPost("GenerateArticleText")]
        public Response<ChatCompletions> GenerateArticleText(AzurePromptInput azurePromptInput)
        {
            // Create text generation service instance
            ITextGenerationService textGenerationService = _factory.CreateTextGenerationService(_client, _config);
            // Create user interface instance and inject text generation service
            UserInterface userInterface = new UserInterface(textGenerationService);
            // Call methods on user interface to interact with the application
            return userInterface.GenerateText(azurePromptInput);
        }

        [HttpPost("GenerateText")]
        public Response<ChatCompletions> GenerateText(AzurePromptInput azurePromptInput)
        {
            // Create text generation service instance
            ITextGenerationService textGenerationService = _factory.CreateTextGenerationService(_client, _config);
            // Create user interface instance and inject text generation service
            UserInterface userInterface = new UserInterface(textGenerationService);
            // Call methods on user interface to interact with the application
            return userInterface.GenerateText(azurePromptInput);
        }
    }
}
