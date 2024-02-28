// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Core.Shared;
using Newtonsoft.Json;

namespace ConnectToAi.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : BasePageModel
    {
        public ConfirmEmailModel(ConfigService configService) :
            base(configService)
        {

        }
        //private readonly UserManager<IdentityUser> _userManager;

        //public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        //{
        //    //_userManager = userManager;
        //}

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }


            
            var url = $"{ApiBaseURL}{APIs.ConfirmEmail}/?userId=" + userId + "&token=" + code;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        StatusMessage = response.IsSuccessStatusCode ? "Thank you for confirming your email." : "Error confirming your email.";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            //var user = await _userManager.FindByIdAsync(userId);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{userId}'.");
            //}

            //code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            //var result = await _userManager.ConfirmEmailAsync(user, code);
            //StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}
