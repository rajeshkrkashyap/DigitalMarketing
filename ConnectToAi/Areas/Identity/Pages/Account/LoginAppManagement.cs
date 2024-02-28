using ConnectToAi.Services;
using ConnectToAi.Util;
using Core.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;

using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static System.Net.WebRequestMethods;

namespace ConnectToAi.Areas.Identity.Pages.Account
{
    public class LoginAppManagement : BasePageModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public LoginRegisterMobileViewModel Input { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        private readonly ILogger<LoginAppModel> _applogger;
 
        private readonly AuthService _authService;

        public LoginAppManagement(ILogger<LoginAppModel> logger,  AuthService authService, ConfigService configService) :
         base(configService)
        {
            _applogger = logger;
            _authService = authService;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var selectedCountryValue = Input.CountryCode;
            if (!TryValidateModel(Input))
            {
                return null;
            }

            //validate OTP from SMS API
            if (Input.OTP.ToString().Length < 6 && Input.OTP.ToString().Length > 6 &&
                Input.MobileNumber.ToString().Length < 6 && Input.MobileNumber.ToString().Length > 6)
            {
                return null;
            }

            if (!OtpManager.VerifyOtp(Input.SessionId, Input.OTP))
            {
                TempData["OTPNotMatch"] = "OPT not match";
                TempData["SessionId"] = Input.SessionId;
                TempData["MobileNumber"] = Input.MobileNumber;
                TempData["CountryCode"] = Input.CountryCode;
                return RedirectToAction("Loginapp", "Account"); //"OTP not match"; 
            }


            LoginRegisterMobileViewModel loginViewModel = new LoginRegisterMobileViewModel
            {
                MobileNumber = Input.MobileNumber,
                OTP = Input.OTP.ToString(),
                CountryCode = Input.CountryCode,
                SessionId = Input.SessionId
            };

            var response = await _authService.MobileLoginAsync(loginViewModel);
            if (response != null)
            {
                return await LoginOnPostAsync(response);
            }
            return Page();
        }
        private async Task<IActionResult> LoginOnPostAsync(HttpResponseMessage response)
        {
            if (response != null && response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody != null)
                {
                    var mainResponse = JsonConvert.DeserializeObject<MainResponse>(responseBody);

                    if (mainResponse != null && mainResponse.IsSuccess)
                    {
                        if (mainResponse.Content != null)
                        {
                            var authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(mainResponse.Content.ToString());
                            if (authenticationResponse != null)
                            {
                                var handler = new JwtSecurityTokenHandler();
                                var jsontoken = handler.ReadToken(authenticationResponse.AccessToken) as JwtSecurityToken;
                                if (!string.IsNullOrWhiteSpace(authenticationResponse.AccessToken))
                                {
                                    string userID = jsontoken.Claims.FirstOrDefault(f => f.Type == JwtRegisteredClaimNames.NameId).Value;
                                    string name = jsontoken.Claims.FirstOrDefault(f => f.Type == JwtRegisteredClaimNames.UniqueName).Value;
                                    string userAvatar = jsontoken.Claims.FirstOrDefault(f => f.Type == "UserAvatar").Value;
                                    string role = jsontoken.Claims.FirstOrDefault(f => f.Type == "role").Value;
                                    string mobileNo = jsontoken.Claims.FirstOrDefault(f => f.Type == "MobileNumber").Value;
                                    string balanceTokens = jsontoken.Claims.FirstOrDefault(f => f.Type == "BalanceTokens").Value;
                                    string subscriptionEndDate = jsontoken.Claims.FirstOrDefault(f => f.Type == "SubscriptionEndDate").Value;
                                    string language = jsontoken.Claims.FirstOrDefault(f => f.Type == "Language").Value;
                                    string countryCode = jsontoken.Claims.FirstOrDefault(f => f.Type == "CountryCode").Value;
                                    string gender = jsontoken.Claims.FirstOrDefault(f => f.Type == "Gender").Value;
                                    string email = "";// UserName;

                                    var userDetail = new UserDetail
                                    {
                                        Email = email,
                                        Name = name,
                                        Role = role,
                                        AccessToken = authenticationResponse.AccessToken,
                                        RefreshToken = authenticationResponse.RefreshToken,
                                        UserAvatar = !string.IsNullOrWhiteSpace(userAvatar) ? $"{ApiBaseURL}/{userAvatar}" : "",
                                        UserID = userID,
                                        Tokens = Convert.ToDecimal(balanceTokens),
                                        Language = language,
                                        CountryCode = countryCode,
                                        Gender = gender

                                        //AppSettingCookie = appSettingCookie
                                    };

                                    string userDetailInfoStr = JsonConvert.SerializeObject(userDetail);
                                    return await UserCookiesManagement(null, subscriptionEndDate, userDetail);
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Invalid Token created");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (mainResponse != null && mainResponse.ErrorMessage != null)
                        {
                            ModelState.AddModelError(string.Empty, mainResponse.ErrorMessage);
                        }
                    }
                }
            }
            else
            {
                if (response != null && response.ReasonPhrase != null)
                {
                    ModelState.AddModelError(string.Empty, response.ReasonPhrase);
                }
            }

            return null;
        }
        private async Task<IActionResult> UserCookiesManagement(string returnUrl, string subscriptionEndDate, UserDetail userDetail)
        {
            string userDetailInfoStr = JsonConvert.SerializeObject(userDetail);

            try
            {
                HttpContext.Response.Cookies.Delete("ConnectToAi_DigitalMarketing_AuthToken");
                HttpContext.Response.Cookies.Delete("ReturnURL");
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1000), // Set cookie expiration date
                    SameSite = SameSiteMode.None,      // Specify same site policy
                    Secure = true                      // Use secure cookie (HTTPS)
                };

                HttpContext.Response.Cookies.Append("ConnectToAi_DigitalMarketing_AuthToken", userDetailInfoStr, cookieOptions);
                //await AppUserSettingCookie(subscriptionEndDate, userDetail);
                _applogger.LogInformation("User logged in.");

                if (userDetail.Role.ToLower() == "marketing")
                {
                    return LocalRedirect("/marketing/Analysing/index");
                }

                else if (userDetail.Role.ToLower() == "admin")
                {
                    return LocalRedirect("/admin/Instruction/index");
                }
                else if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = Url.Content("~/");
                }

                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
      
    }
}
