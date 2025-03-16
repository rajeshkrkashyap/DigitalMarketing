using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSLibrary
{
    internal class NexmoSmsSender : ISmsSender
    {
        public bool SendSms(string mobileNumber, string countryCode, string textMessage)
        {
            // Implement Nexmo SMS sending logic here
            Console.WriteLine($"Sending SMS using Nexmo to {countryCode}{mobileNumber}: {textMessage}");
            return false;
        }
    }

    // Create a con
}
