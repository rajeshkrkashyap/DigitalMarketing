using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Api.Models;
using Core.Shared.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using Twilio.TwiML.Voice;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RechargeController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public RechargeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("List")]
        public List<Recharge> LoadAllRecharges()
        {
            return _dbContext.Recharges.ToList();
        }
        
        [HttpPost("GetByUserId")]
        public List<Recharge> GetRechargeByUserId(string userId)
        {
            return _dbContext.Recharges.Where(inst => inst.AppUserId == userId).OrderByDescending(o=>o.Created).Take(5).ToList();
        }

        [HttpPost("GetById")]
        public Recharge GetRecharge(string id)
        {
            return _dbContext.Recharges.FirstOrDefault(inst => inst.Id == id);
        }

        [HttpPost("GetByRazorpayPaymentId")]
        public Recharge GetByRazorpayPaymentId(string razorpay_payment_id)
        {
            return _dbContext.Recharges.Include(i=>i.AppUser).FirstOrDefault(inst => inst.Razorpay_payment_id == razorpay_payment_id);
        }
        [HttpPost("GetByCurrency")]
        public Recharge GetRechargeByCurrency(string currency)
        {
            return _dbContext.Recharges.FirstOrDefault(inst => inst.Currency == currency);
        }

        [HttpPost("Create")]
        public Recharge Create(Recharge Recharge)
        {
            if (Recharge != null)
            {
                _dbContext.Recharges.Add(new Recharge
                {
                    Id = Recharge.Id,
                    AppUserId = Recharge.AppUserId,
                    ApplicationServiceId = Recharge.ApplicationServiceId,
                    RechargeAmount = Recharge.RechargeAmount,
                    Currency = Recharge.Currency,
                    Tokens = Recharge.Tokens,
                    OrderId = Recharge.OrderId,
                    Razorpay_order_id = Recharge.Razorpay_order_id,
                    Razorpay_payment_id = Recharge.Razorpay_payment_id,
                    Razorpay_signature = Recharge.Razorpay_signature,
                    SubscriptionType = Recharge.SubscriptionType,
                    SubscriptionStartDate = Recharge.SubscriptionStartDate,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    IsActive = true,
                });
                _dbContext.SaveChanges();
                return Recharge;
            }
            return null;
        }

        [HttpPost("Update")]
        public bool Update(Recharge Recharge)
        {
            if (Recharge != null)
            {
                var dbRecharge = _dbContext.Recharges.FirstOrDefault(inst => inst.Id == Recharge.Id);

                dbRecharge.Currency = Recharge.Currency;
                dbRecharge.RechargeAmount = Recharge.RechargeAmount;
                dbRecharge.Updated = DateTime.UtcNow;
                _dbContext.Recharges.Update(dbRecharge);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        [HttpPost("Delete")]
        public bool Delete(string id)
        {
            if (id != null)
            {
                var Recharge = _dbContext.Recharges.FirstOrDefault(inst => inst.Id == id);
                Recharge.IsActive = false;
                _dbContext.Recharges.Update(Recharge);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
