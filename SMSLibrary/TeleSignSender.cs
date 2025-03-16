using log4net;
using System.Net.Http.Json;
//using Telesign;

namespace SMSLibrary
{
    internal class TeleSignSender : ISmsSender
    {
      
        public bool SendSms(string mobileNumber, string countryCode, string textMessage)
        {
            //        // Replace the defaults below with your Telesign authentication credentials.
            //        string customerId = "0104D86A-63FE-4BC9-A1CF-9D3BF50A2278";
            //        string apiKey = "ZBd7GlNDY8BKdFQCDV/+XnJ7VFJbJGJ2P24IO9YkoO9mDQuuDOkDGG1x5z1sM8cS+u/ja6port7dOV8MputJeg==";

            //        // Set the default below to your test phone number. 
            //        // In your production code, update the phone number dynamically for each transaction.    
            //        string phoneNumber = "918448846917";
            //        string responseBody = "Failed";
            //        //// (Optional) Pull values from environment variables instead of hardcoding them.
            //        //if (System.Environment.GetEnvironmentVariable("CUSTOMER_ID") != null)
            //        //{
            //        //    customerId = System.Environment.GetEnvironmentVariable("CUSTOMER_ID");
            //        //}

            //        //if (System.Environment.GetEnvironmentVariable("API_KEY") != null)
            //        //{
            //        //    apiKey = System.Environment.GetEnvironmentVariable("API_KEY");
            //        //}

            //        //if (System.Environment.GetEnvironmentVariable("PHONE_NUMBER") != null)
            //        //{
            //        //    phoneNumber = System.Environment.GetEnvironmentVariable("PHONE_NUMBER");
            //        //}

            //        try
            //        {
            //            // Instantiate a Phone ID client object.
            //            PhoneIdClient phoneIdClient = new PhoneIdClient(customerId, apiKey);

            //            // Make the request and capture the response.
            //            RestClient.TelesignResponse telesignResponse = phoneIdClient.PhoneId(phoneNumber);
            //            responseBody =telesignResponse.Body;
            //            // Display the response in the console for debugging purposes. 
            //            // In your production code, you would likely remove this.
            //            Console.WriteLine("\nResponse HTTP status:\n" + telesignResponse.StatusCode);
            //            Console.WriteLine("\nResponse body:\n" + responseBody);

            //        }
            //        catch (Exception e)
            //        {
            //            Console.ForegroundColor = ConsoleColor.Red;
            //            Console.WriteLine("\nAn exception occured.\nERROR: " + e.Message + "\n");
            //            Console.ResetColor();
            //        }

            //        //Console.WriteLine("Press any key to quit.");
            //        //Console.ReadKey();

            return false;
        }
    }
}
