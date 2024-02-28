// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using ConnectToAi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace ConnectToAi.Areas.Identity.Pages.Account
{
    public class LoginAppModel : LoginAppManagement
    {
        public LoginAppModel(ILogger<LoginAppModel> logger, AuthService authService, ConfigService configService) : base(logger, authService, configService)
        {
            Input = new LoginRegisterMobileViewModel();
        }

        public async Task OnGetAsync(string countryCode,  string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            TempData["CountryCode"] = countryCode;
            Input.CountryCode = countryCode;
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;
        }

    }
}
