namespace ConnectToAi.Util
{
    public class OtpManager
    {
        private static Dictionary<string, Tuple<string, DateTime>> userOtps = new Dictionary<string, Tuple<string, DateTime>>();
        private static readonly object lockObject = new object();

        public static string GenerateOtp(string userId)
        {
            lock (lockObject)
            {
                // Generate a random 6-digit OTP
                Random random = new Random();
                string generatedOtp = random.Next(100000, 999999).ToString();

                // Set OTP expiration to 15 minutes from now
                DateTime otpExpiration = DateTime.Now.AddMinutes(15);

                // Store OTP and its expiration for the user
                userOtps[userId] = new Tuple<string, DateTime>(generatedOtp, otpExpiration);

                return generatedOtp;
            }
        }

        public static bool VerifyOtp(string? userId, string? userEnteredOtp)
        {
            lock (lockObject)
            {
                // Check if OTP exists for the user
                if (userOtps.TryGetValue(userId, out Tuple<string, DateTime> userOtp))
                {
                    // Check if OTP is still valid
                    if (DateTime.Now <= userOtp.Item2)
                    {
                        // Compare user-entered OTP with the generated OTP
                        bool isOtpValid = userEnteredOtp.Equals(userOtp.Item1, StringComparison.OrdinalIgnoreCase);

                        // Remove OTP entry after verification
                        userOtps.Remove(userId);

                        return isOtpValid;
                    }
                }

                return false; // OTP is either not generated for the user or has expired
            }
        }
    }
}
