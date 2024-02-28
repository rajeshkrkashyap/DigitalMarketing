using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DAL.Server;
using DAL;
using System;

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
                        Crawler crawler = new Crawler();
                        crawler.SeedUrl = project.URL;
                        var projectId = crawler.Crawl();
                        projectService.ProjectUpdateStatus(projectId, "Completed");

                    });
                }
            }
        }
    }
}