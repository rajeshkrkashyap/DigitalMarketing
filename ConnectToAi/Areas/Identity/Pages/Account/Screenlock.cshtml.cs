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
    public class ScreenlockModel : BasePageModel
    {
        private readonly ILogger<ScreenlockModel> _logger;
 
        public ScreenlockModel(ILogger<ScreenlockModel> logger, ConfigService configService)
            :base(configService)
        {
            _logger = logger;
        }

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

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            Request.Cookies.TryGetValue("ReturnURL", out string? returnURLCookieValue);
            if (returnURLCookieValue != null)
            {
                TempData["ReturnUrl"] = returnURLCookieValue;
            }

            Request.Cookies.TryGetValue("ConnectToAi_DigitalMarketing_AuthToken", out string? cookieValue);
            if (cookieValue != null)
            {
                var userDetail = JsonConvert.DeserializeObject<UserDetail?>(cookieValue);
                TempData["Email"] = userDetail.Email;
                TempData["Name"] = userDetail.Name;
            }

            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
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
                                                return UserCookiesManagement(returnUrl);
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

            //Request.Cookies.TryGetValue("ReturnURL", out string? returnURLCookieValue);
            //if ((returnURLCookieValue != null && !returnURLCookieValue.ToString().Contains(Request.Path)) || returnURLCookieValue == null)
            //{
            //    //TempData["ReturnUrl"] = Request.Headers["Referer"].ToString();
            //    SetReturnURLCookies(Request.Headers["Referer"].ToString());

            //    Request.Cookies.TryGetValue("ReturnURL", out string? returnURL);
            //    TempData["ReturnUrl"] = returnURL;
            //}

            Request.Cookies.TryGetValue("ConnectToAi_DigitalMarketing_AuthToken", out string? cookieValue);
            if (cookieValue != null)
            {
                var userDetail = JsonConvert.DeserializeObject<UserDetail?>(cookieValue);
                TempData["Email"] = userDetail.Email;
                TempData["Name"] = userDetail.Name;
            }
            return Page();
        }

        private IActionResult UserCookiesManagement(string returnUrl)
        {
            try
            {
                _logger.LogInformation("User logged in.");
                HttpContext.Response.Cookies.Delete("ReturnURL");
                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SetReturnURLCookies(string returnUrl)
        {
            try
            {
                HttpContext.Response.Cookies.Delete("ReturnURL");

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1), // Set cookie expiration date
                    SameSite = SameSiteMode.None,      // Specify same site policy
                    Secure = true                      // Use secure cookie (HTTPS)
                };

                HttpContext.Response.Cookies.Append("ReturnURL", returnUrl, cookieOptions);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
