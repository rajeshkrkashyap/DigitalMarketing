using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SMSLibrary
{
    internal class DigiMilesSmsSender : ISmsSender
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DigiMilesSmsSender));

        public HttpResponseMessage SendSms(string mobileNumber, string countryCode, string textMessage)
        {
            // Implement Digi miles SMS sending logic here
            Console.WriteLine($"Sending SMS using DigiMiles to {countryCode}{mobileNumber}: {textMessage}");
            return SendSMSHttpClient(mobileNumber, countryCode, textMessage);
        }

        private static HttpResponseMessage SendSMSHttpClient(string mobileNumber, string countryCode, string textMessage)
        {
            // DEFINE PARAMETERS USED IN URL
            // To what server you need to connect to for submission
            // i.e. string Server = "xxxxx.xxxxx.xxxxx";
            string Server = "route.digimiles.in";
            // Port that is to be used like 8080 or 8000
            string Port = "8443";

            // Username that is to be used for submission
            // i.e. string UserName = "tester";
            string UserName = "DG35-royall";

            // Password that is to be used along with username
            // i.e. string Password = "password";
            string Password = "digimile";

            // What type of message that is to be sent.
            // 0: means plain text
            // 1: means flash
            // 2: means Unicode (Message content should be in Hex)
            // 6: means Unicode Flash (Message content should be in Hex)
            int type = 0;

            // Message content that is to be transmitted
            string Message = "Dear User, Your one time password " + textMessage + " and its valid for 15 minutes only. Do not share to anyone. Thank you,  ConnectTo.Ai (Powered by Royall Vastramm)";

            // Url Encode message
            Message = HttpUtility.UrlEncode(Message);

            if (type == 2 || type == 6)
            {
                Message = ConvertToUnicode(Message);
            }

            string Entityid = "1101555800000075635";
            string Tempid = "1107170607414744896";

            // Require DLR or not
            // 0: means DLR is not Required
            // 1: means DLR is Required
            int DLR = 1;

            // Sender Id to be used for submitting the message
            // i.e. string SenderName = "test";
            string Source = "ROYLVS";

            // Destinations to which message is to be sent
            // For submitting more than one destination at once,
            // destinations should be comma separated like '91999000123,91999000124'
            string Destination = mobileNumber;

            //string URL = $"https://{Server}:{Port}?type={type}&dlr={DLR}&destination={Destination}&source={Source}&message={Message}&entityid=1101555800000075635&tempid=1107170607414744896";

            string URL = $"http://{Server}/bulksms/bulksms?username={UserName}&password={Password}&type={type}&dlr={DLR}&destination={Destination}&source={Source}&message={Message}&entityid={Entityid}&tempid={Tempid}";
            //https://rslri.connectbind.com:8443/bulksms/bulksms?username=DG35-royall&password=digimile&type=0&dlr=1&destination=9916011355&source=ROYLVS&message=Dear User, Your one time password 567845 and its valid for 15 minutes only. Do not share to anyone. Thank you,  ConnectTo.Ai (Powered by Royall Vastramm)&entityid=1101555800000075635&tempid=1107170607414744896
            //http://route.digimiles.in/bulksms/bulksms?username=DG35-royall&password=digimile&type=0&dlr=1&destination=9916011355&source=ROYLVS&message=Dear User, Your one time password 567845 and its valid for 15 minutes only. Do not share to anyone. Thank you,  ConnectTo.Ai (Powered by Royall Vastramm)&entityid=1101555800000075635&tempid=1107170607414744896


            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    log.Info("SMS service is calling");
                    return httpClient.GetAsync(URL).Result;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private static string SendSMS(string mobileNumber, string countryCode, string textMessage)
        {
            WebRequest webRequest; // object for WebRequest
            WebResponse webResponse; // object for WebResponse

            // DEFINE PARAMETERS USED IN URL
            // To what server you need to connect to for submission
            // i.e. string Server = "xxxxx.xxxxx.xxxxx";
            string Server = "rslri.connectbind.com";
            // Port that is to be used like 8080 or 8000
            string Port = "8443";

            // Username that is to be used for submission
            // i.e. string UserName = "tester";
            string UserName = "DG35-royall";

            // Password that is to be used along with username
            // i.e. string Password = "password";
            string Password = "digimile";

            // What type of message that is to be sent.
            // 0: means plain text
            // 1: means flash
            // 2: means Unicode (Message content should be in Hex)
            // 6: means Unicode Flash (Message content should be in Hex)
            int type = 0;

            // Message content that is to be transmitted
            string Message = "Dear User, Your one time password " + textMessage + " and its valid for 15 minutes only. Do not share to anyone. Thank you,  ConnectTo.Ai (Powered by Royall Vastramm)";

            // Url Encode message
            Message = HttpUtility.UrlEncode(Message);

            if (type == 2 || type == 6)
            {
                Message = ConvertToUnicode(Message);
            }

            string Entityid = "1101555800000075635";
            string Tempid = "1107170607414744896";

            // Require DLR or not
            // 0: means DLR is not Required
            // 1: means DLR is Required
            int DLR = 1;

            // Sender Id to be used for submitting the message
            // i.e. string SenderName = "test";
            string Source = "ROYLVS";

            // Destinations to which message is to be sent
            // For submitting more than one destination at once,
            // destinations should be comma separated like '91999000123,91999000124'
            string Destination = mobileNumber;

            //CODE COMPLETE TO DEFINE PARAMETER
            string WebResponseString = "";
            //string URL = $"https://{Server}:{Port}?type={type}&dlr={DLR}&destination={Destination}&source={Source}&message={Message}&entityid=1101555800000075635&tempid=1107170607414744896";

            string URL = $"https://{Server}:{Port}/bulksms/bulksms?username={UserName}&password={Password}&type={type}&dlr={DLR}&destination={Destination}&source={Source}&message={Message}&entityid={Entityid}&tempid={Tempid}";
            //https://rslri.connectbind.com:8443/bulksms/bulksms?username=DG35-royall&password=digimile&type=0&dlr=1&destination=9916011355&source=ROYLVS&message=Dear User, Your one time password 567845 and its valid for 15 minutes only. Do not share to anyone. Thank you,  ConnectTo.Ai (Powered by Royall Vastramm)&entityid=1101555800000075635&tempid=1107170607414744896

            webRequest = WebRequest.Create(URL); // Hit URL Link
            webRequest.Timeout = 25000;

            try
            {
                webResponse = webRequest.GetResponse(); // Get Response
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    // Read Response and store in variable
                    WebResponseString = reader.ReadToEnd();
                }
                webResponse.Close();
                //Response.Write(WebResponseString); // Display Response.
                return WebResponseString;
            }
            catch (Exception ex)
            {
                WebResponseString = "Request Timeout"; // If any exception occur.
                //Response.Write(WebResponseString);
                return WebResponseString;
            }
        }

        // Function To Convert String to Unicode if MessageType=2 and 6.
        private static string ConvertToUnicode(string str)
        {
            byte[] arrayOfBytes = System.Text.Encoding.Unicode.GetBytes(str);
            string unicodeString = "";
            for (int v = 0; v < arrayOfBytes.Length; v++)
            {
                if (v % 2 == 0)
                {
                    int t = arrayOfBytes[v];
                    arrayOfBytes[v] = arrayOfBytes[v + 1];
                    arrayOfBytes[v + 1] = (byte)t;
                }
            }
            for (int v = 0; v < arrayOfBytes.Length; v++)
            {
                string c = arrayOfBytes[v].ToString("X");
                if (c.Length == 1)
                {
                    c = "0" + c;
                }
                unicodeString += c;
            }
            return unicodeString;
        }
    }

}
