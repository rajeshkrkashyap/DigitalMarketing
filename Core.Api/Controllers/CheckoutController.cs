using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Shared;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static string s_wasmClientURL = string.Empty;
        private string visitorIPAddrtess = string.Empty;
        private string currencyCode = string.Empty;
        private decimal amount = 0;

        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<ActionResult> CheckoutOrder([FromBody] CheckOutProduct checkOutProduct, [FromServices] IServiceProvider sp)
        {
            s_wasmClientURL = _configuration["AppUrl"]; 
            visitorIPAddrtess = checkOutProduct.IpAddress; 

            if (visitorIPAddrtess == "::1" || visitorIPAddrtess == "127.0.0.1")
            {
                var visitorIPAddrtess = await GetMyLocalMachinePublicIPAddress();
                var countryName = await GetCountryName(visitorIPAddrtess);
                currencyCode = GetCurrencyCode(countryName);
            }
            else
            {
                currencyCode = await GetCountryName(visitorIPAddrtess);
            }
            
            var product = checkOutProduct.Product;

            amount = await GetCurrencyConverter("USD", currencyCode, product.Price);

            // Build the URL to which the customer will be redirected after paying.
            var server = sp.GetRequiredService<IServer>();

            var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

            string? thisApiUrl = null;

            if (serverAddressesFeature is not null)
            {
                thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();
            }

            if (thisApiUrl is not null)
            {


                var sessionId = await CheckOut(product, thisApiUrl);
                var pubKey = _configuration["Stripe:PubKey"];

                var checkoutOrderResponse = new CheckoutOrderResponse()
                {
                    SessionId = sessionId,
                    PubKey = pubKey
                };

                return Ok(checkoutOrderResponse);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [NonAction]
        public async Task<string> CheckOut(Shared.Entities.Product product, string thisApiUrl)
        {


            // Create a payment flow from the items in the cart.
            // Gets sent to Stripe API.
            var options = new SessionCreateOptions
            {
                // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                SuccessUrl = $"{s_wasmClientURL}/success?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                CancelUrl = s_wasmClientURL + "failed",  // Checkout cancelled.
                PaymentMethodTypes = new List<string> // Only card available in test mode?
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)amount, //product.Price, // Price is in USD cents.
                        Currency = currencyCode, // "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Title,
                            Description = product.Description,
                            Images = new List<string> { product.ImageUrl }
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
            };

            try
            {


                var service = new SessionService();
                var session = await service.CreateAsync(options);
                return session.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet("success")]
        // Automatic query parameter handling from ASP.NET.
        // Example URL: https://localhost:7051/checkout/success?sessionId=si_123123123123
        public ActionResult CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            // Here you can save order and customer details to your database.
            var total = session.AmountTotal.Value;
            var customerEmail = session.CustomerDetails.Email;

            return Redirect(s_wasmClientURL + "success");
        }

        private async Task<string> GetCountryName(string visitorIPAddress)
        {
            string apiKey = _configuration["DetectCunteryApi"]; //Replace with your actual API key from ipstack
            string apiUrl = $"http://api.ipstack.com/{visitorIPAddress}?access_key={apiKey}";

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(apiUrl);
                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                    string countryName = data.country_name;
                    string countryCode = data.country_code;

                    Console.WriteLine($"Country Name: {countryName}");
                    Console.WriteLine($"Country Code: {countryCode}");

                    return countryName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            throw new Exception("Currency can't be convert");
        }
        private async Task<decimal> GetCurrencyConverter(string baseCurrency, string targetCurrency, decimal amount)
        {
            string apiKey = _configuration["correncyConverterApi"];  //Replace with your actual API key from exchangeratesapi.io

            string apiUrl = $"https://open.er-api.com/v6/latest/{baseCurrency}?apikey={apiKey}";

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(apiUrl);
                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                    decimal exchangeRate = data.rates[targetCurrency];
                    decimal convertedAmount = amount * exchangeRate;

                    Console.WriteLine($"{amount} {baseCurrency} is equal to {convertedAmount} {targetCurrency}");

                    return convertedAmount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            throw new Exception("Currency can't be convert");
        }
        private async Task<string> GetMyLocalMachinePublicIPAddress()
        {
            using (var client = new HttpClient())
            {
                string apiUrl = "https://api.ipify.org?format=json";
                var response = await client.GetStringAsync(apiUrl);

                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                string publicIP = data.ip;

                Console.WriteLine($"Your public IP address is: {publicIP}");
                return publicIP;
            }
        }
        private static string GetCurrencyCode(string countryName)
        {
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo region = new RegionInfo(ci.Name);
                if (region.EnglishName.Equals(countryName, StringComparison.OrdinalIgnoreCase))
                {
                    return region.ISOCurrencySymbol;
                }
            }
            return "Currency code not found.";
        }
    }
}
