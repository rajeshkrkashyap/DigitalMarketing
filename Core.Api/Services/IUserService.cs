 
using Core.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography;
using Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Twilio.Types;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Core.Api.Models;

namespace Core.Api.Services
{
    public interface IUserService
    {
        Task<MainResponse> SendOtp(string mobileNumber, string countryCode);
        Task<MainResponse> RegisterAndLoginByUserMobileAsync(LoginRegisterMobileViewModel model);
        Task<MainResponse> RegisterUserAsync(RegisterViewModel model);
        Task<MainResponse> LoginUserAsync(LoginViewModel model);
        Task<MainResponse> ConfirmEmailAsync(string userId, string token);
        Task<MainResponse> ForgetPasswordAsync(string email);
        Task<MainResponse> ResetPasswordAsync(Core.Shared.ResetPasswordViewModel model);
        Task<MainResponse> RefreshTokenAsync(AuthenticationResponse model);
        Task<MainResponse> GetUserByEmail(string email);
        Task<MainResponse> IsEmailConfirmedAsync(AppUser user);
        Task<MainResponse> GeneratePasswordResetTokenAsync(AppUser user);
        Task<MainResponse> AddToRole(string userId, string role);
    }

    public class UserService : IUserService
    {

        private UserManager<AppUser> _userManger;
        private IConfiguration _configuration;
        private IMailService _mailService;
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IConfiguration configuration, IMailService mailService)
        {
            _userManger = userManager;
            _configuration = configuration;
            _mailService = mailService;
            _dbContext = dbContext;
        }

        public async Task<MainResponse> SendOtp(string mobileNumber, string countryCode)
        {
            var user = await _userManger.FindByNameAsync(mobileNumber.ToString());
            if (user == null)
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "This mobile number is not register!"
                };
            }

            OTPManager otpManager = new OTPManager(_dbContext);
            string userId = user.Id;
            //Generate OTP
            string generatedOTP = otpManager.GenerateOTP(userId);
            var status = SMSLibrary.SmsServiceProvider.SendSMS(mobileNumber, countryCode, generatedOTP);

            return new MainResponse
            {
                IsSuccess = true,
                Content = status
            };
        }

        #region "Mobile Login and Registration"
        public async Task<MainResponse> RegisterAndLoginByUserMobileAsync(LoginRegisterMobileViewModel model)
        {
            var user = await _userManger.FindByNameAsync(model.MobileNumber.ToString());
            if (user == null)
            {
                await RegisterVaiMobileNumberAsync(model);
            }
            return await LoginVaiMobileNumberAsync(model);
        }
        private async Task<MainResponse> RegisterVaiMobileNumberAsync(LoginRegisterMobileViewModel model)
        {
            if (model.Role.ToLower() == "admin")
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Please choose a valid role"
                };
            }

            if (model == null)
                throw new NullReferenceException("Reigster Model is null");


            //var user = await _userManger.FindByNameAsync(model.MobileNumber);

            //if (user != null)
            //{
            //    await _userManger.UpdateAsync(user);
            //}

            var AppUser = new AppUser
            {
                PhoneNumber = model.MobileNumber.ToString(),
                UserName = model.MobileNumber.ToString(),
                PhoneNumberConfirmed = true,
                Language = "English",
                BalanceToken = 100000,  //Free token provided by CoonectTo.Ai
                SubscriptionEndDate = DateTime.UtcNow.AddDays(10),
            };
            try
            {
                var result = await _userManger.CreateAsync(AppUser);

                if (result.Succeeded)
                {

                    //var isAddedInRole = await _userManger.AddToRoleAsync(AppUser, "student");
                    var userAddedInRole = await AddToRole(AppUser.Id, model.Role);
                    if (!userAddedInRole.IsSuccess)
                    {
                        return userAddedInRole;
                    }
                    //var confirmEmailToken = await _userManger.GenerateEmailConfirmationTokenAsync(AppUser);
                    //var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                    //var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                    ////string url = $"{_configuration["AppUrl"]}/Identity/Account/confirmemail?userid={AppUser.Id}&code={validEmailToken}";
                    //string url = $"{model.EmailConfirmUrl}?userid={AppUser.Id}&code={validEmailToken}";
                    //await _mailService.SendEmailAsync(AppUser.Email, "Confirm your email", $"<h1>Welcome to Connectto.Ai </h1>" +
                    //$"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");
                    //Send registration message to Mobile Number

                    OTPManager otpManager = new OTPManager(_dbContext);
                    string userId = AppUser.Id;
                    // Generate OTP
                    string generatedOTP = otpManager.GenerateOTP(userId);
                    SMSLibrary.SmsServiceProvider.SendSMS(model.MobileNumber, model.CountryCode, generatedOTP);

                    return new MainResponse
                    {
                        IsSuccess = true,
                        Content = "User created successfully!"
                    };
                }
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User did not create",
                };
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }

        }
        private async Task<MainResponse> LoginVaiMobileNumberAsync(LoginRegisterMobileViewModel model)
        {

            var user = await _userManger.FindByNameAsync(model.MobileNumber.ToString());

            if (user == null)
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "There is no user with that Mobile number"
                };
            }

            var iseMobileNumberConfirmed = await _userManger.IsPhoneNumberConfirmedAsync(user);

            if (!iseMobileNumberConfirmed)
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Verify your Mobile number!",
                };
            }

            OTPManager otpManager = new OTPManager(_dbContext);
            var isOtpVerified = otpManager.VerifyOTP(user.Id, Convert.ToInt32(model.OTP));
            if (!isOtpVerified)
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Wrong OTP!",
                };
            }

            string accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            await _userManger.UpdateAsync(user);

            var response = new MainResponse
            {
                Content = new AuthenticationResponse
                {
                    RefreshToken = refreshToken,
                    AccessToken = accessToken,
                },
                IsSuccess = true,
                ErrorMessage = ""
            };
            return response;
        }
        #endregion

        public async Task<MainResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model.Role.ToLower() == "admin")
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Please choose a valid role"
                };
            }

            if (model == null)
                throw new NullReferenceException("Reigster Model is null");

            if (model.Password != model.ConfirmPassword)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Confirm password doesn't match the password"
                };

            var AppUser = new AppUser
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                BalanceToken = 100000,  //Free token provided by CoonectTo.Ai
                SubscriptionEndDate = DateTime.UtcNow.AddDays(10),
            };
            try
            {
                var result = await _userManger.CreateAsync(AppUser, model.Password);

                if (result.Succeeded)
                {

                    //var isAddedInRole = await _userManger.AddToRoleAsync(AppUser, "student");
                    var userAddedInRole = await AddToRole(AppUser.Id, model.Role);
                    if (!userAddedInRole.IsSuccess)
                    {
                        return userAddedInRole;
                    }
                    var confirmEmailToken = await _userManger.GenerateEmailConfirmationTokenAsync(AppUser);

                    var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                    var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                    //string url = $"{_configuration["AppUrl"]}/Identity/Account/confirmemail?userid={AppUser.Id}&code={validEmailToken}";
                    string url = $"{model.EmailConfirmUrl}?userid={AppUser.Id}&code={validEmailToken}";

                    await _mailService.SendEmailAsync(AppUser.Email, "Confirm your email", $"<h1>Welcome to Connectto.Ai </h1>" +
                        $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");

                    return new MainResponse
                    {
                        IsSuccess = true,
                        Content = "User created successfully!"
                    };
                }
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User did not create",
                };
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }

        }
        public async Task<MainResponse> LoginUserAsync(LoginViewModel model)
        {

            var user = await _userManger.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "There is no user with that Email address"
                };
            }

            var isemailConfirmed = await _userManger.IsEmailConfirmedAsync(user);

            if (!isemailConfirmed)
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Email not Confirmed!",
                };
            }
            var result = await _userManger.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid password",
                };

            var claims = new[]
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            string accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            await _userManger.UpdateAsync(user);
            //var appUserSetting = _dbContext.AppUserSettings.Include(i => i.Subject).FirstOrDefault(id => id.AppUserId == user.Id && id.IsQueryActive == true);

            var response = new MainResponse
            {
                Content = new AuthenticationResponse
                {
                    RefreshToken = refreshToken,
                    AccessToken = accessToken,
                },
                IsSuccess = true,
                ErrorMessage = ""
            };
            return response;
        }
        public async Task<MainResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if (user == null)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found"
                };

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManger.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
                return new MainResponse
                {
                    IsSuccess = true,
                    Content = "Email confirmed successfully!",
                };

            return new MainResponse
            {
                IsSuccess = false,
                ErrorMessage = "Email did not confirm",
                Content = result.Errors.Select(e => e.Description),
            };
        }
        public async Task<MainResponse> ForgetPasswordAsync(string email)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if (user == null)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "No user associated with email",
                };

            var token = await _userManger.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_configuration["AppUrl"]}/ResetPassword?email={email}&token={validToken}";

            await _mailService.SendEmailAsync(email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
                $"<p>To reset your password <a href='{url}'>Click here</a></p>");

            return new MainResponse
            {
                IsSuccess = true,
                Content = "Reset password URL has been sent to the email successfully!"
            };
        }
        public async Task<MainResponse> ResetPasswordAsync(Core.Shared.ResetPasswordViewModel model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user == null)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "No user associated with email",
                };


            if (model.NewPassword != model.ConfirmPassword)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Password doesn't match its confirmation",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManger.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
                return new MainResponse
                {
                    IsSuccess = true,
                    Content = "Password has been reset successfully!",
                };

            return new MainResponse
            {
                IsSuccess = false,
                Content = result.Errors.Select(e => e.Description)
            };
        }
        public async Task<MainResponse> RefreshTokenAsync(AuthenticationResponse refreshTokenRequest)
        {
            var principal = GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);

            if (principal != null)
            {
                var claimObj = principal.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email || f.Type == ClaimTypes.NameIdentifier);

                var user = await _userManger.FindByIdAsync(claimObj?.Value);

                if (user is null /*|| user.RefreshToken != refreshTokenRequest.RefreshToken*/)
                {
                    return new MainResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Invalid Request",
                    };
                }

                string newAccessToken = GenerateAccessToken(user);
                string refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                await _userManger.UpdateAsync(user);

                return new MainResponse
                {
                    IsSuccess = true,
                    Content = new AuthenticationResponse
                    {
                        RefreshToken = refreshToken,
                        AccessToken = newAccessToken
                    }
                };
            }
            else
            {
                return new MainResponse
                {
                    IsSuccess = true,
                    ErrorMessage = "Invalid Token Found"
                };
            }
        }
        public async Task<MainResponse> GetUserByEmail(string email)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if (user == null)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "No user associated with email",
                };


            return new MainResponse
            {
                IsSuccess = true,
                Content = user
            };
        }
        public async Task<MainResponse> IsEmailConfirmedAsync(AppUser user)
        {
            var status = await _userManger.IsEmailConfirmedAsync(user);
            if (!status)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "No user email is not confirm, Please confirm!",
                };

            return new MainResponse
            {
                IsSuccess = true,
                Content = status
            };
        }
        public async Task<MainResponse> GeneratePasswordResetTokenAsync(AppUser user)
        {
            var code = await _userManger.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(code))
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "coude could not generated!",
                };

            return new MainResponse
            {
                IsSuccess = true,
                Content = code
            };
        }
        private string GenerateAccessToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var phoneNumber = user.PhoneNumber != null ? user.PhoneNumber : "";
            var email = user.Email != null ? user.Email : "";

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, $"{user.Name}"),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.MobilePhone, phoneNumber),
                    new Claim("MobileNumber", phoneNumber),
                    new Claim("UserAvatar", $"{user.UserAvatar}"),
                    new Claim("BalanceTokens", $"{user.BalanceToken}"),
                    new Claim("SubscriptionEndDate", $"{user.SubscriptionEndDate}"),
                    new Claim("Language", $"{user.Language}"),
                    new Claim("CountryCode", $"{user.CountryCode}"),
                    new Claim("Gender", $"{user.Gender}"),
            };

            var roles = _userManger.GetRolesAsync(user);

            foreach (var role in roles.Result)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            string keyFromConfig = _configuration["JWT:Key"];

            // Ensure the key has at least 256 bits (32 bytes)
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyFromConfig.PadRight(32, '\0'));


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration["JWT:Audience"],
                Issuer = _configuration["JWT:Issuer"],
                Expires = DateTime.UtcNow.AddDays(1000),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyDetail = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenValidationParameter = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(keyDetail),
            };

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameter, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
        private string GenerateRefreshToken()
        {

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public async Task<MainResponse> AddToRole(string userId, string role)
        {
            if (role.ToLower() == "admin")
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User can't become Admin"
                };
            }

            var user = await _userManger.FindByIdAsync(userId);
            if (user == null)
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found"
                };
            }
            var result = await _userManger.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return new MainResponse
                {
                    IsSuccess = true,
                    Content = "Added to role successfully!",
                };
            }
            return new MainResponse
            {
                IsSuccess = false,
                ErrorMessage = "Failed to add in role!",
                Content = result.Errors.Select(e => e.Description),
            };
        }

    }

}
