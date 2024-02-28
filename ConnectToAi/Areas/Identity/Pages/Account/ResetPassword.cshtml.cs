// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Core.Shared.Entities;
using Core.Shared;
using Newtonsoft.Json;
using Core.Api.Models;

namespace ConnectToAi.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : BasePageModel
    {
        public ResetPasswordModel(ConfigService configService) :
            base(configService)
        {

        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public ResetPasswordViewModel Input { get; set; }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                //Input = new InputModel
                //{
                //    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                //};
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

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
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            url = $"{ApiBaseURL}{APIs.ResetPasswordAsync}";
            string message = "";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var jsonData = JsonConvert.SerializeObject(Input);
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
                                    message = JsonConvert.DeserializeObject<string>(mainResponse.Content.ToString());
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
            //var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (message == "Password has been reset successfully!")
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }
            ModelState.AddModelError(string.Empty, message);
            return Page();
        }
    }
}
