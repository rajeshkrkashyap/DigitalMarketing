namespace Core.Api.Models
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;

    public class OTPManager
    {
        private readonly string connectionString;
        private readonly ApplicationDbContext _dbContext;
        public OTPManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GenerateOTP(string userId)
        {
            Random random = new Random();
            int otp = random.Next(100000, 999999); // Generate a random 6-digit OTP
            DateTime expiryTime = DateTime.Now.AddMinutes(15); // OTP will expire after 15 minutes

            StoreOTPInDatabase(userId, otp, expiryTime);
            return otp.ToString("D6"); // Format OTP to 6 digits
        }
        public bool VerifyOTP(string userId, int otp)
        {

            if (RetrieveOTPFromDatabase(userId, otp))
            {
                RemoveOTPFromDatabase(userId);
                return true;
            }
            return false; // OTP verification failed
        }

        private void StoreOTPInDatabase(string userId, int otp, DateTime expiryTime)
        {
            try
            {
                _dbContext.OneTimePasswords.Add(new Shared.Entities.OneTimePassword { UserId = userId, OTP = otp, ExpiryTime = expiryTime });
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
        private bool RetrieveOTPFromDatabase(string userId, int otp)
        {
            try
            {
                var oneTimePasswords = _dbContext.OneTimePasswords.Where(o => o.UserId == userId && o.OTP == otp).ToList();
                if (oneTimePasswords.Count() > 0)
                {
                    if (oneTimePasswords.Where(t => t.ExpiryTime >= DateTime.Now).Count() > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        private void RemoveOTPFromDatabase(string userId)
        {
            try
            {
                var otp = _dbContext.OneTimePasswords.Find(userId);
                _dbContext.OneTimePasswords.Remove(otp);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
