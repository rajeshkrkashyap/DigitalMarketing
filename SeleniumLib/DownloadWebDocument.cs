using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace SeleniumLib
{
    public class WebDocument
    {
        private static IWebDriver _driver;
        private static readonly object LockObject = new object();

        static WebDocument()
        {
            // Set the path to the ChromeDriver executable
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string driverPath = assemblyPath.Replace("SeleniumLib.dll", "") + @"\chrome\";

            // Set ChromeDriverService path
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(driverPath);

            // Set up ChromeOptions (optional: enable headless mode)
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");

            // Create a single ChromeDriver instance
            _driver = new ChromeDriver(chromeDriverService, chromeOptions);
        }

        public static string DownloadPageSource(string url)
        {
            try
            {
                lock (LockObject)
                {
                    // Navigate to a website
                    _driver.Navigate().GoToUrl(url);
                    // Perform interactions (e.g., click buttons, fill forms)
                    // Capture the page source (HTML content)
                    string htmlContent = _driver.PageSource;

                    return htmlContent;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
