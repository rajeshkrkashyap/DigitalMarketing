using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSLibrary
{
    internal class TwilioSmsSender : ISmsSender
    {
        public bool SendSms(string mobileNumber, string countryCode, string textMessage)
        {
            // Implement Twilio SMS sending logic here
            Console.WriteLine($"Sending SMS using Twilio to {countryCode}{mobileNumber}: {textMessage}");
            return false;
        }
    }

}
