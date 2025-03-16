using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Core.Service.Server;
using Core.Service;
using System;
using Serilog;
using Newtonsoft.Json;
using Core.Shared.Entities;
using Core.Shared;
using HtmlAgilityPack;

namespace OnPageSsoScoreServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Set up the server socket
            TcpListener server = new TcpListener(IPAddress.Any, Constants.OnPageSeoScoreServerPORT); // Listening on port 12345
            server.Start();

            Console.WriteLine("\nServer started, waiting for connections...");
            OnPageSeoScore onPageSeoScore = new OnPageSeoScore(new HtmlDocument(), new ProjectService(), new LoggerConfiguration());
            CrawledService crawledService = new CrawledService();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                try
                {
                    Console.WriteLine("\nClient connected");
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"\nReceived: {message}");

                    // Start the crawler with the received URL
                    StartServer(message, crawledService, onPageSeoScore);

                    // Respond to the client
                    byte[] response = Encoding.ASCII.GetBytes("\nServer received your message.");
                    stream.Write(response, 0, response.Length);

                    // Clean up
                    stream.Close();
                    client.Close();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    // Clean up
                    stream.Close();
                    client.Close();
                }
            }
        }

        static async void StartServer(string message, CrawledService crawledService, OnPageSeoScore onPageSeoScore)
        {
            if (!string.IsNullOrEmpty(message))
            {
                string[] messageArray = message.Split(Constants.SpliterIdentifier);
                string projectId = messageArray[0];
                string pageUrl = messageArray[1];
                var crawled = await crawledService.GetPageContent(projectId, pageUrl);
                if (crawled != null)
                {
                    // Add logic to start your crawler with the provided URL
                    await Task.Run(() =>
                    {
                        // This code will run asynchronously on a separate thread
                        onPageSeoScore.Crawled = crawled;
                        Thread.Sleep(5000);
                    });
                }
            }
        }
    }
}