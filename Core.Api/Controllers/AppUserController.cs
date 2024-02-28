using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Api.Models;
using Core.Shared;
using Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Twilio.TwiML.Voice;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public AppUserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("List")]
        public List<AppUser> LoadAllAppUsers()
        {
            return _dbContext.Users.ToList();
        }

        [HttpPost("GetById")]
        public AppUser GetAppUser(string id)
        {
            return _dbContext.Users.FirstOrDefault(inst => inst.Id == id);
        }

        [HttpPost("UpdateTokens")]
        public AppUser UpdateTokens(AppUserViewModel appUserViewModel)
        {
            if (string.IsNullOrEmpty(appUserViewModel.Id))
            {
                return null;
            }
            var user = _dbContext.Users.FirstOrDefault(inst => inst.Id == appUserViewModel.Id);
            if (user != null)
            {
                user.BalanceToken += appUserViewModel.Tokens;
                user.SubscriptionEndDate = appUserViewModel.SubscriptionEndDate;
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
            }
            return user;
        }

        [HttpPost("Update")]
        public AppUser Update(AppUser appUser)
        {
            if (string.IsNullOrEmpty(appUser.PhoneNumber))
            {
                return null;
            }
            var user = _dbContext.Users.FirstOrDefault(inst => inst.PhoneNumber == appUser.PhoneNumber);
            if (user != null)
            {
                user.Name = appUser.Name;
                user.Gender = appUser.Gender;
                user.CountryCode = appUser.CountryCode;
                user.Language = appUser.Language;
                user.SubscriptionEndDate = appUser.SubscriptionEndDate;
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
            }
            return user;
        }
    }
}
