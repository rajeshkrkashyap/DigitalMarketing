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
using System.Net.Sockets;
using System.Text;
using NUnit.Framework;
using ConnectToAi.Controllers;

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
        public BaseController(ConfigService configService, ProjectService projectService) : base(configService, projectService)
        {

        }
        public BaseController(ConfigService configService, ProjectService projectService, ArticleTypeService articleTypeService) : base(configService, projectService, articleTypeService)
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
        public UserDetail? UserDetail
        {
            get
            {
                Request.Cookies.TryGetValue("ConnectToAi_DigitalMarketing_AuthToken", out string? cookieValue);
                if (cookieValue != null)
                {
                    return JsonConvert.DeserializeObject<UserDetail?>(cookieValue);
                }
                return null;
            }
        }
        protected bool SendProjectForAnalysis(string id)
        {
            TcpClient client = new();
            try
            {
                client.Connect("127.0.0.1", 12345); //Connect to server at IP address 127.0.0.1 (localhost) and port 12345
                NetworkStream stream = client.GetStream();
                string message = id;    //projectId //"https://royallvastramm.com" ;
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received from server: {response}");
                stream.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                client.Close();
            }
        }
    }
}
