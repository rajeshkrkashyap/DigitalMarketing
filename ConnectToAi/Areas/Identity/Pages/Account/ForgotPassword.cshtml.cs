// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConnectToAi.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : BasePageModel
    {
        public ForgotPasswordModel(ConfigService configService) :
            base(configService)
        {

        }

        //private readonly IEmailSender _emailSender;

        //public ForgotPasswordModel(IEmailSender emailSender)
        //{
        //    _emailSender = emailSender;
        //}

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                AppUser user = null;
                var url = $"{ApiBaseURL}{APIs.GetUserByEmail}/?email=" + Input.Email;
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
                                    return NotFound($"Unable to load user with email '{Input.Email}'.");
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

                bool status = false;
                url = $"{ApiBaseURL}{APIs.IsEmailConfirmedAsync}";
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var jsonData = JsonConvert.SerializeObject(user);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

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
                                        status =  (bool)mainResponse.Content;
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


                if (user == null || !status)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                string code = "";
                url = $"{ApiBaseURL}{APIs.GeneratePasswordResetTokenAsync}";
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var jsonData = JsonConvert.SerializeObject(user);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

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
                                        code = mainResponse.Content.ToString();
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
               
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
