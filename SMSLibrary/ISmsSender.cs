using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSLibrary
{
    internal interface ISmsSender
    {
        HttpResponseMessage SendSms(string mobileNumber, string countryCode, string textMessage=null);

    }
}
