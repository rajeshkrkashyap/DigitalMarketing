using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Core.Api.Models;
using Core.Api.Services;
using Core.Shared;
using Core.Shared.Entities;
using System;
using System.IO;
using System.Threading.Tasks;
using SMSLibrary;
namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private IMailService _mailService;
        private IConfiguration _configuration;
        public AuthController(IUserService userService, IMailService mailService, IConfiguration configuration)
        {
            _userService = userService;
            _mailService = mailService;
            _configuration = configuration;
        }
        
        // /api/auth/login
        [HttpPost("MobileLogin")]
        public async Task<IActionResult> MobileLoginAsync([FromBody] LoginRegisterMobileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterAndLoginByUserMobileAsync(model);
                if (result.IsSuccess)
                {
                    //await _mailService.SendEmailAsync(model.MobileNumber, "New login", "<h2>Hey!, new login to your account noticed</h2><p>New login to your account at " + DateTime.Now + "</p>");
                    return Ok(result);
                }
                return Ok(result);
            }

            return BadRequest("Some properties are not valid");
        }

        // /api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result); // Status Code: 200 

                return Ok(result);
            }

            return BadRequest("Some properties are not valid"); // Status code: 400
        }

        // /api/auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);

                if (result.IsSuccess)
                {
                    await _mailService.SendEmailAsync(model.Email, "New login", "<h2>Hey!, new login to your account noticed</h2><p>New login to your account at " + DateTime.Now + "</p>");
                    return Ok(result);
                }

                return Ok(result);
            }

            return BadRequest("Some properties are not valid");
        }

        // /api/auth/confirmemail?userid&token
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // api/auth/forgetpassword
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _userService.ForgetPasswordAsync(email);

            if (result.IsSuccess)
                return Ok(result); // 200

            return BadRequest(result); // 400
        }

        // api/auth/resetpassword
        [HttpPost("ResetPasswordAsync")]
        public async Task<MainResponse> ResetPasswordAsync([FromForm] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                return await _userService.ResetPasswordAsync(model);
            }
            return new MainResponse
            {
                IsSuccess = false,
                ErrorMessage = "Some properties are not valid",
            };
        }

        // api/auth/refreshToken
        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<MainResponse> RefreshTokenAsync(AuthenticationResponse refreshTokenRequest)
        {
            if (refreshTokenRequest is null)
            {
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid Request",
                };
            }

            return await _userService.RefreshTokenAsync(refreshTokenRequest);
        }

        [AllowAnonymous]
        [HttpPost("GetUserByEmail")]
        public async Task<MainResponse> GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Enter valid format of Email!"
                };

            return await _userService.GetUserByEmail(email);
        }

        [AllowAnonymous]
        [HttpPost("IsEmailConfirmedAsync")]
        public async Task<MainResponse> IsEmailConfirmedAsync(AppUser user)
        {
            if (user == null)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "user is null"
                };

            return await _userService.IsEmailConfirmedAsync(user);
        }

        [AllowAnonymous]
        [HttpPost("GeneratePasswordResetTokenAsync")]
        public async Task<MainResponse> GeneratePasswordResetTokenAsync(AppUser user)
        {
            if (user == null)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "user is null"
                };

            return await _userService.GeneratePasswordResetTokenAsync(user);
        }
 
        [HttpPost("AddToRoleAsync")]
        public async Task<MainResponse> AddToRole(string userId,string role)
        {
            if (userId == null)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "user is null"
                };
            if (role == null)
                return new MainResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "role is null"
                };

            return await _userService.AddToRole(userId, role);
        }
        private async Task<string> UploadFile(byte[] bytes, string fileName)
        {
            string uploadsFolder = Path.Combine("Images", fileName);
            Stream stream = new MemoryStream(bytes);
            using (var ms = new FileStream(uploadsFolder, FileMode.Create))
            {
                await stream.CopyToAsync(ms);
            }
            return uploadsFolder;
        }

    }
}