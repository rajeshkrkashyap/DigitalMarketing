using ConnectToAi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Shared;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace ConnectToAi.Areas.Identity.Pages.Account
{
    public class LoginManagement : BasePageModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public LoginViewModel Input { get; set; }

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

        private readonly ILogger<LoginModel> _logger;
        private readonly ILogger<LoginAppModel> _applogger;


        public LoginManagement(ILogger<LoginModel> logger, ConfigService configService) :
            base(configService)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(Input);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var url = $"{ApiBaseURL}{APIs.Login}";
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(url, content);

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
                                                string email = Input.Email;

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
                                                    //AppSettingCookie = appSettingCookie
                                                };

                                                return await UserCookiesManagement(returnUrl, subscriptionEndDate, userDetail);
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
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
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
                    Expires = DateTime.Now.AddDays(1), // Set cookie expiration date
                    SameSite = SameSiteMode.None,      // Specify same site policy
                    Secure = true                      // Use secure cookie (HTTPS)
                };

                HttpContext.Response.Cookies.Append("ConnectToAi_DigitalMarketing_AuthToken", userDetailInfoStr, cookieOptions);
                _logger.LogInformation("User logged in.");

                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = Url.Content("~/");
                }
                if (userDetail.Role.ToLower() == "avatar")
                {
                    return LocalRedirect("/avatar/settings/index");
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
