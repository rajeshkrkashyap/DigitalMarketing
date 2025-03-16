using ConnectToAi.Filters;
using ConnectToAi.Recharges;
using ConnectToAi.Services;
using Microsoft.AspNetCore.Mvc;
using Core.Shared;
using Newtonsoft.Json;

namespace ConnectToAi.Controllers
{
    public class CommonBaseController : Controller
    {
        protected readonly ConfigService? _configService;
        protected readonly BlobStorageService? _blobStorageService;
        protected readonly OcrService? _ocrService;
        protected readonly ServiceService? _service;
        protected readonly RechargeService? _rechargeService;
        protected readonly AppUserService? _appUserService;
        protected readonly IAppService? _appService;
        protected readonly AuthService? _authService;
        protected readonly ProjectService? _projectService;
        protected readonly BlogService? _blogService;
        protected readonly ArticleTypeService _articleTypeService;
        public CommonBaseController()
        {

        }
        public CommonBaseController(ConfigService configService)
        {
            _configService = configService;
        }
        public CommonBaseController(ConfigService configService, BlogService blogService)
        {
            _configService = configService;
            _blogService = blogService;
        }
        public CommonBaseController(ConfigService configService, ProjectService projectService)
        {
            _configService = configService;
            _projectService = projectService;
        }
        public CommonBaseController(ConfigService configService, ProjectService projectService, ArticleTypeService articleTypeService)
        {
            _configService = configService;
            _projectService = projectService;
            _articleTypeService= articleTypeService;
        }
        public CommonBaseController(AuthService authService, RechargeService rechargeService, AppService appService, AppUserService appUserService, ConfigService configService)
        {
            _appService = appService;
            _appUserService = appUserService;
            _configService = configService;
            _rechargeService = rechargeService;
            _authService = authService;
        }

        public CommonBaseController(AppUserService appUserService, ConfigService configService) :
            this(configService)
        {
            _appUserService = appUserService;
        }


        public CommonBaseController(AppUserService appUserService, ServiceService service, RechargeService rechargeService) :
            this(null)
        {
            _service = service;
            _rechargeService = rechargeService;
        }


        public string ApiBaseURL { get { return _configService.AppSettings.ApiBaseUrl; } }

        public UserDetail? AppUserDetail
        {
            get
            {
                Request.Cookies.TryGetValue("ConnectToAi_DigitalMarketing_AuthToken", out string? cookieValue);
                if (cookieValue != null)
                {
                    var userDetail = JsonConvert.DeserializeObject<UserDetail?>(cookieValue);
                    ViewBag.Role = userDetail.Role;
                    return userDetail;
                }
                return null;
            }
        }
    }
}
