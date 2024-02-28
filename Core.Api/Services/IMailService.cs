using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace Core.Api.Services
{
    public interface IMailService
    {

        Task<bool> SendEmailAsync(string toEmail, string subject, string content);
    }

    public class SendGridMailService : IMailService
    {
        private IConfiguration _configuration;

        public SendGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = "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"; //_configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("postmaster@connectto.ai", "Connect to Ai");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);

            return true;
            //SendEmailForVarification(toEmail, subject, content);
            //await SendVerificationEmailVaiMandril(toEmail, subject, content);
        }
        private async Task SendVerificationEmailVaiMandril(string toEmail, string subject, string body)
        {
            string apiKey = "md-Zi3eonuX5BMZxVkdpxwpeg"; // Replace with your Mandrill API key
            string mandrillEndpoint = "https://mandrillapp.com/api/1.0/messages/send.json";

            HttpClient client = new HttpClient();

            try
            {
                // Prepare the message
                var content = new StringContent(
                 @"{
                    ""key"": """ + apiKey + @""",
                    ""message"": {
                        ""text"": """ + body + @""",
                        ""subject"": """ + subject + @""",
                        ""from_email"": ""postmaster@connectto.ai"",
                        ""to"": [
                            {
                                ""email"": """ + toEmail + @""",
                                ""name"": """ + toEmail + @""",
                                ""type"": ""to""
                            }
                        ]
                    }
                }",
                 Encoding.UTF8,
                 "application/json"
             );

                // Send the request
                HttpResponseMessage response = await client.PostAsync(mandrillEndpoint, content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Email sent successfully!");
                }
                else
                {
                    Console.WriteLine("Error: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }
        private async Task SendEmaiVaiMandrillapp(string toEmail, string subject, string content)
        {
            string smtpHost = "smtp.mandrillapp.com";
            int smtpPort = 587; // Update with the appropriate SMTP port
            string smtpUsername = "ConnectToAi";
            string smtpPassword = "md-Zi3eonuX5BMZxVkdpxwpeg"; //ApiKey As a Password
            string senderEmail = "postmaster@connectto.ai";
            string recipientEmail = toEmail;//"Rajesh.kr.kashyap@gmail.com";

            try
            {
                // Create a new MailMessage
                MailMessage mail = new MailMessage(senderEmail, recipientEmail, subject, content);

                // Create an instance of the SmtpClient
                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                // Send the email
                await smtpClient.SendMailAsync(mail);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error: {ex.Message}");
            }
        }
        private async Task SendEmaiVaiMailChimp(string toEmail, string subject, string content)
        {
            string apiKey = "bc14a214caad2048c38e5562bb0af073-us14";
            string ansotherApiKey = "7de533ca973c0e6ddec6dc2f26e6649232762f02"; https://app.sparkpost.com/account/api-keys

            string serverPrefix = "YOUR_SERVER_PREFIX";
            string listId = "3e464119b4";
            string recipientEmail = "rajesh.kr.kashyap@gmail.com";
            string senderEmail = "ConnectTo2023@gmail.com";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"https://{serverPrefix}.api.mailchimp.com/3.0/");

            // Prepare the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"lists/{listId}/members");
            request.Headers.Add("Authorization", $"apikey {apiKey}");

            // Build the request body
            var requestBody = new
            {
                email_address = recipientEmail,
                status = "pending",
                merge_fields = new
                {
                    FNAME = "Recipient",
                    LNAME = "Lastname"
                }
            };

            request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            // Send the request
            HttpResponseMessage response = await client.SendAsync(request);

            // Check the response status
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Verification email sent successfully!");
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to send verification email. Error: {errorMessage}");
            }

            client.Dispose();
        }

        private async Task SendEmailForVarification(string toEmail, string subject, string body)
        {

            string smtpHost = "mail5019.site4now.net\r\n";
            int smtpPort = 465; // Update with the appropriate SMTP port
                                //string smtpUsername = "SMTP_Injection";
                                //string smtpPassword = "7de533ca973c0e6ddec6dc2f26e6649232762f02"; //ApiKey As a Password
            string senderEmail = "postmaster@connectto.ai";
            string recipientEmail = toEmail;//"Rajesh.kr.kashyap@gmail.com";


            //string smtpHost = "smtp.sparkpostmail.com";
            //int smtpPort = 587; // Update with the appropriate SMTP port
            //string smtpUsername = "SMTP_Injection";
            //string smtpPassword = "7de533ca973c0e6ddec6dc2f26e6649232762f02"; //ApiKey As a Password
            //string senderEmail = "ConnectTo@mamtastore.com";
            //string recipientEmail = toEmail;//"Rajesh.kr.kashyap@gmail.com";

            try
            {
                // Create a new MailMessage
                MailMessage mail = new MailMessage(senderEmail, recipientEmail, subject, body);

                // Create an instance of the SmtpClient
                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
                smtpClient.EnableSsl = true;
                // smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                // Send the email
                await smtpClient.SendMailAsync(mail);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error: {ex.Message}");
            }
        }
    }
}
