using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Shared;

namespace ConnectToAi.Areas.Identity.Pages.Account
{
    public class BasePageModel : PageModel
    {
        protected readonly ConfigService _configService;
        public BasePageModel(ConfigService configService)
        {
            _configService = configService;
        }
        public string ApiBaseURL { get { return _configService.AppSettings.ApiBaseUrl; } }

    }

}
