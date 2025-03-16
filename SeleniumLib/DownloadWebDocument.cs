using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
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

        public static string DownloadPageSource(string url, out string visitedUrl)
        {
            try
            {
                lock (LockObject)
                {
                    // Navigate to a website
                    _driver.Navigate().GoToUrl(url);
                    // Perform interactions (e.g., click buttons, fill forms)
                    // Capture the page source (HTML content)
                    visitedUrl = _driver.Url;
                    string htmlContent = _driver.PageSource;

                    return htmlContent;
                }
            }
            catch (Exception ex)
            {
                visitedUrl = null;
                return null;
            }
        }

        public static TimeSpan MeasurePageLoadingSpeed(string url)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                // Navigate to the target web page
                driver.Navigate().GoToUrl(url);
                // Start measuring time before navigating to the page
                DateTime startTime = DateTime.Now;

                // Wait for the page to fully load
                ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete");

                // Calculate the time taken for the page to load
                TimeSpan loadingTime = DateTime.Now - startTime;
                return loadingTime;
            }
        }


        public static bool IsMobileFriendly(string url)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Url = url;
                // Resize the browser window to simulate a mobile device viewport
                driver.Manage().Window.Size = new System.Drawing.Size(375, 667); // iPhone 6/7/8 viewport size

                // Wait for the page to fully load
                ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete");

                // Check if elements on the page are responsive
                bool isResponsive = AreElementsResponsive(driver);

                //// Check if the URL contains mobile-specific keywords (optional)
                //bool containsMobileKeywords = CheckForMobileKeywords(driver.Url);

                // Determine mobile friendliness based on responsiveness and other criteria
                //bool isMobileFriendly = isResponsive && containsMobileKeywords;

                return isResponsive;
            }
        }

        static bool AreElementsResponsive(IWebDriver driver)
        {
            // Placeholder logic for demonstration purposes
            // You can replace this with actual checks specific to your web page
            // For instance, you could look for specific CSS classes or elements that are indicative of responsive design
            try
            {  // Define a list of responsive CSS classes
                string[] responsiveClasses = {
                    "d-none",        // Hide element on all screen sizes
                    "d-inline",      // Display element inline on all screen sizes
                    "d-inline-block", // Display element inline block on all screen sizes
                    "d-block",       // Display element as block on all screen sizes
                    "d-sm-none",     // Hide element on small screen (sm) and above
                    "d-md-none",     // Hide element on medium screen (md) and above
                    "d-lg-none",     // Hide element on large screen (lg) and above
                    "d-xl-none",     // Hide element on extra large screen (xl) and above
                    "d-sm-inline",   // Display element inline on small screen and above
                    "d-md-inline",   // Display element inline on medium screen and above
                    "d-lg-inline",   // Display element inline on large screen and above
                    "d-xl-inline",   // Display element inline on extra large screen and above
                    "d-sm-inline-block", // Display element inline block on small screen and above
                    "d-md-inline-block", // Display element inline block on medium screen and above
                    "d-lg-inline-block", // Display element inline block on large screen and above
                    "d-xl-inline-block", // Display element inline block on extra large screen and above
                    "d-sm-block",    // Display element as block on small screen and above
                    "d-md-block",    // Display element as block on medium screen and above
                    "d-lg-block",    // Display element as block on large screen and above
                    "d-xl-block",    // Display element as block on extra large screen and above
                    "d-sm-flex",     // Display element as flex on small screen and above
                    "d-md-flex",     // Display element as flex on medium screen and above
                    "d-lg-flex",     // Display element as flex on large screen and above
                    "d-xl-flex",     // Display element as flex on extra large screen and above
                    "d-sm-table",    // Display element as table on small screen and above
                    "d-md-table",    // Display element as table on medium screen and above
                    "d-lg-table",    // Display element as table on large screen and above
                    "d-xl-table"     // Display element as table on extra large screen and above
                };

                // Check if any element with a responsive CSS class is displayed
                foreach (string responsiveClass in responsiveClasses)
                {
                    try
                    {
                        if (driver.FindElement(By.CssSelector($".{responsiveClass}")).Displayed)
                        {
                            return true;
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        // Continue to the next class if the current class is not found
                        continue;
                    }
                }

                // Check if certain elements are visible or hidden based on screen size
                //bool isResponsive = driver.FindElement(By.CssSelector(".responsive-element")).Displayed;
                //return isResponsive;
                return CheckForMobileKeywords(driver.Url);
            }
            catch (NoSuchElementException)
            {
                // If the responsive element is not found, consider the page not responsive
                return false;
            }
        }

        static bool CheckForMobileKeywords(string url)
        {
            // Define keywords that indicate the URL is mobile-friendly
            string[] mobileKeywords = { "mobile", "m.", "mobi", "responsive", "adaptive" };

            // Check if the URL contains any mobile-specific keywords
            foreach (string keyword in mobileKeywords)
            {
                if (url.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
