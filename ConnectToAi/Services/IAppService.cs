using Core.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConnectToAi.Services
{
    public interface IAppService
    {
        Task<string> RefreshToken(UserDetail userDetail);
        public Task<AuthUserResponse> AuthenticateUser(LoginViewModel loginModel, string apiBaseUrl);
    }
}
