using ConnectToAi.Areas.Admin.Controllers;
using ConnectToAi.Filters;
using ConnectToAi.Recharges;
using ConnectToAi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using Stripe;
using Stripe.Terminal;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    [Area("Marketing")]
    [AuthorizationFilter("Marketing")]
    [ActionFilter]
    public class BaseController : CommonBaseController
    {
        public BaseController()
        {

        }
        public BaseController(ConfigService configService) : base(configService)
        {

        }
       
        public BaseController(  AppUserService appUserService, ConfigService configService) :
            base(appUserService, configService)
        {

        }
       

        public BaseController(AppUserService appUserService, ServiceService service, RechargeService rechargeService) :
            base(appUserService, service, rechargeService)
        {

        }
    }
}
