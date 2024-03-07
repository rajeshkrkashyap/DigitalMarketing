using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DAL.Server;
using DAL;
using System;
using Serilog;
using Newtonsoft.Json;
using Core.Shared.Entities;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Set up the server socket
            TcpListener server = new TcpListener(IPAddress.Any, 12345); // Listening on port 12345
            server.Start();

            Console.WriteLine("\nServer started, waiting for connections...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("\nClient connected");

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"\nReceived: {message}");

                // Start the crawler with the received URL
                StartCrawler(message);

                // Respond to the client
                byte[] response = Encoding.ASCII.GetBytes("\nServer received your message.");
                stream.Write(response, 0, response.Length);

                // Clean up
                stream.Close();
                client.Close();
            }
        }

        static async void StartCrawler(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ProjectService projectService = new ProjectService();
                var project = projectService.GetProjectById(id);
                if (project != null && !string.IsNullOrEmpty(project.Id))
                {
                    // Add logic to start your crawler with the provided URL
                    await Task.Run(() =>
                    {
                        // Add your crawler logic here
                        // This code will run asynchronously on a separate thread
                        string jsonFilePath = "ignore_keywords.json"; // Replace with the path to your JSON file
                        var _ignoreWordList = ReadJsonToList<string>(jsonFilePath);
                        Crawler crawler = new Crawler(new ProjectService(), new CrawledService(),new ContentAnalysis(_ignoreWordList, new ContentAnalysisService()), new LoggerConfiguration());
                        crawler.SeedUrl = project.URL;
                        crawler.UrlsToIgnore = LoadIgnoreUrls("ignore_urls.json");
                        AddIgnoreList(crawler, project.URL);
                        Thread.Sleep(5000);
                        var projectId = crawler.Crawl();
                        projectService.ProjectUpdateStatus(projectId, "Completed");
                    });
                }
            }
        }
        private static List<T>? ReadJsonToList<T>(string filePath)
        {
            string json = System.IO.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        //Methods for specific actions would be defined here
        static List<string> LoadIgnoreUrls(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<string>>(jsonContent);
        }

        private static void AddIgnoreList(Crawler crawler, string seedUrl)
        {
            var baseUri = new Uri(seedUrl);
            //update Ignore urls list
            crawler.UrlsToIgnore.Add("^https?://" + baseUri.Host + "#");
            crawler.UrlsToIgnore.Add("^https?://" + baseUri.Host + "/#");
            crawler.UrlsToIgnore.Add("^https?://" + baseUri.Host + "/login");
            crawler.UrlsToIgnore.Add("^https?://" + baseUri.Host + "/auth");
            crawler.UrlsToIgnore.Add("^https?://" + baseUri.Host + "/register");
            crawler.UrlsToIgnore.Add("^https?://" + baseUri.Host + "/registeration");

            crawler.UrlsToIgnore.Add("^https?://" + baseUri.Host + "/account/login");
            crawler.UrlsToIgnore.Add("^https?://" + baseUri.Host + "/account/auth");
            crawler.UrlsToIgnore.Add("^https?://" + baseUri.Host + "/account/register");
            crawler.UrlsToIgnore.Add("^https?://" + baseUri.Host + "/account/registeration");

            crawler.UrlsToIgnore.Add(@"^https:\/\/" + baseUri.Host + @"\/page\/\d+$");
            crawler.UrlsToIgnore.Add(@"^https:\/\/" + baseUri.Host + @"\/page\/\d+$");
            crawler.UrlsToIgnore.Add(@"^https:\/\/" + baseUri.Host + @"\/page\/\d+$");
            crawler.UrlsToIgnore.Add(@"^https:\/\/" + baseUri.Host + @"\/pagenumber\/\d+$");
            
            crawler.UrlsToIgnore.Add(@"^https:\/\/" + baseUri.Host + @"\/products\?page=\d+$");
            crawler.UrlsToIgnore.Add(@"^https:\/\/" + baseUri.Host + @"\/products\/page\/\d+$");
            crawler.UrlsToIgnore.Add(@"^https:\/\/" + baseUri.Host + @"\/products\?page=[\w-]+$");
            
            crawler.UrlsToIgnore.Add("^https?://www." + baseUri.Host + "/login");
            crawler.UrlsToIgnore.Add("^https?://www." + baseUri.Host + "/auth");
            crawler.UrlsToIgnore.Add("^https?://www." + baseUri.Host + "/register");
            crawler.UrlsToIgnore.Add("^https?://www." + baseUri.Host + "/registeration");
            crawler.UrlsToIgnore.Add("^https?://www." + baseUri.Host + "/account/login");
            crawler.UrlsToIgnore.Add("^https?://www." + baseUri.Host + "/account/auth");
            crawler.UrlsToIgnore.Add("^https?://www." + baseUri.Host + "/account/register");
            crawler.UrlsToIgnore.Add("^https?://www." + baseUri.Host + "/account/registeration");
            crawler.UrlsToIgnore.Add(@"^https:\/\/www" + baseUri.Host + @"/page\/\d+$");
            crawler.UrlsToIgnore.Add(@"^https:\/\/www" + baseUri.Host + @"/page\/\d+$");
            crawler.UrlsToIgnore.Add(@"^https:\/\/www" + baseUri.Host + @"/page\/\d+$");
            crawler.UrlsToIgnore.Add(@"^https:\/\/www" + baseUri.Host + @"/products\?page=\d+$");
            crawler.UrlsToIgnore.Add(@"^https:\/\/www" + baseUri.Host + @"/products\/page\/\d+$");
            crawler.UrlsToIgnore.Add(@"^https:\/\/www" + baseUri.Host + @"/products\?page=[\w-]+$");
            
            crawler.UrlsToIgnore.Add(@"\?cursor=([^&]+)");
            crawler.UrlsToIgnore.Add(@"offset=(\d+)&limit=(\d+)");
            
            // Add exclusion for .js and .css files
            crawler.UrlsToIgnore.Add(@"\.js$");
            crawler.UrlsToIgnore.Add(@"\.css$");
        }
    }
}