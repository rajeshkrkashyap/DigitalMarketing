using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSLibrary
{
    internal interface ISmsSender
    {
        bool SendSms(string mobileNumber, string countryCode, string textMessage=null);

    }
}
