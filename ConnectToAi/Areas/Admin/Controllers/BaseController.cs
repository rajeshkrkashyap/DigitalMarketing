using ConnectToAi.Filters;
using ConnectToAi.Recharges;
using ConnectToAi.Services;
using Microsoft.AspNetCore.Mvc;
using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using Stripe;

namespace ConnectToAi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizationFilter("Admin")]
    [ActionFilter]
    public class BaseController : CommonBaseController
    {

        public BaseController()
        {

        }
        public BaseController(ConfigService configService) : base(configService)
        {

        }

        public BaseController(AppUserService appUserService, ConfigService configService) :
            base(appUserService, configService)
        {

        }


        public BaseController(AppUserService appUserService, ServiceService service, RechargeService rechargeService) :
            base(appUserService, service, rechargeService)
        {

        }


    }
}
