using Azure.AI.OpenAI;
using AzureOpenAiLib;
using Core.Ai;
using Core.Api.Models;
using Core.Shared;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class CompetitorArticleController : ControllerBase
    {
        private readonly CompetitorArticle _competitorArticle;
        public CompetitorArticleController(CompetitorArticle competitorArticle)
        {
            _competitorArticle = competitorArticle;
        }

        [HttpPost("PrimaryKeywords")]
        public async Task<MainResponse> AiGeneratePrimaryKeywords(Blog request)
        {
            var keywords = await _competitorArticle.AiGeneratePrimaryKeywords(request);

            return new MainResponse
            {
                Content = keywords,
                ErrorMessage = "",
                IsSuccess = true
            };
        }

        [HttpPost("SecondaryKeywords")]
        public async Task<MainResponse> AiGenerateSecondaryKeywords(Blog request)
        {
             var keywords = await _competitorArticle.AiGenerateSecondaryKeywords(request);

            return new MainResponse
            {
                Content = keywords,
                ErrorMessage = "",
                IsSuccess = true
            };
        }

        [HttpPost("Content")]
        public async Task<MainResponse> AiGenerateContent(Blog request)
        {
            var content  = await _competitorArticle.AiGenerateContent(request);
            return new MainResponse
            {
                Content = content,
                ErrorMessage = "",
                IsSuccess = true
            };
        }

        [HttpPost("MetaTags")]
        public async Task<MainResponse> AiGenerateMetaTags(Blog request)
        {
            var metaTags =await _competitorArticle.AiGenerateMetaTags(request);
            return new MainResponse
            {
                Content = metaTags,
                ErrorMessage = "",
                IsSuccess = true
            };
        }
    }
}
