using log4net;

namespace SMSLibrary
{
    public class SmsServiceProvider
    {
        public static HttpResponseMessage SendSMS(string mobileNumber, string countryCode, string otp)
        {
            SmsService smsService;
            switch (countryCode)
            {
                case "+":
                    // Example usage
                    smsService = new SmsService(new TeleSignSender());
                    return smsService.SendSms(mobileNumber, "+1", otp);
                case "91":
                    // Example usage
                    smsService = new SmsService(new DigiMilesSmsSender());
                    return smsService.SendSms(mobileNumber, "+91", otp);
                case "1":
                    // Example usage
                    smsService = new SmsService(new TwilioSmsSender());
                    return smsService.SendSms(mobileNumber, "+91", otp);
                case "44":
                    // Example usage
                    smsService = new SmsService(new NexmoSmsSender());
                    return smsService.SendSms(mobileNumber, "+1", otp);
                    
                default:
                    break;
            }

            //var smsServiceTeleSign = new SmsService(new TeleSignSender());
            //return smsServiceTeleSign.SendSms(mobileNumber, "91");
            return null;
        }
    }
}
