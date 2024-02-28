using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSLibrary
{
    // Create a context class that will use the selected SMS sender strategy
    internal class SmsService
    {
        private readonly ISmsSender _smsSender;
        
        public SmsService(ISmsSender smsSender)
        {
            _smsSender = smsSender;
        }
            
        public HttpResponseMessage SendSms(string mobileNumber, string countryCode, string message)
        {
            // You can add additional logic here if needed
            return _smsSender.SendSms(mobileNumber, countryCode, message);
        }
    }
}
