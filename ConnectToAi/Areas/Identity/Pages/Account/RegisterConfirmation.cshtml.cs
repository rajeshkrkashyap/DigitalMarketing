// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;

namespace ConnectToAi.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : BasePageModel
    {
        public RegisterConfirmationModel(ConfigService configService) :
            base(configService)
        {

        }

        //private readonly IEmailSender _sender;

        //public RegisterConfirmationModel(IEmailSender sender)
        //{
        //    _sender = sender;
        //}

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool DisplayConfirmAccountLink { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");
            AppUser user = null;
            var url = $"{ApiBaseURL}{APIs.GetUserByEmail}/?email=" + email;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(url, null);

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
                                    user = JsonConvert.DeserializeObject<AppUser>(mainResponse.Content.ToString());
                                }
                            }

                            if (user == null)
                            {
                                return NotFound($"Unable to load user with email '{email}'.");
                            }

                            Email = email;
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

            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = true;
            //if (DisplayConfirmAccountLink)
            //{
            //    var userId = await _userManager.GetUserIdAsync(user);
            //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //    EmailConfirmationUrl = Url.Page(
            //        "/Account/ConfirmEmail",
            //        pageHandler: null,
            //        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
            //        protocol: Request.Scheme);
            //}

            return Page();
        }
    }
}
