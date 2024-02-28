using Core.Shared.Entities;
using DAL.Server;
using System;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    private static Queue<Project> projectList = new Queue<Project>();
    static void Main(string[] args)
    {
        TcpClient client = null;
        try
        {

            while (true)
            {
                try
                {
                    GetProjectFromDB();

                    if (projectList.Count > 0)
                    {
                        client = null;
                        client = new TcpClient();
                        client.Connect("127.0.0.1", 12345); //Connect to server at IP address 127.0.0.1 (localhost) and port 12345
                        NetworkStream stream = client.GetStream();
                        var project = projectList.Dequeue();
                        Console.WriteLine($"\nDequeue: {project.URL}");
                        string message = project.Id; //"https://royallvastramm.com";
                        byte[] data = Encoding.ASCII.GetBytes(message);
                        stream.Write(data, 0, data.Length);

                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"\nReceived from server: {response}");
                        stream.Close();
                        Console.WriteLine($"\nLast request triggered at: {DateTime.UtcNow}");
                        Console.WriteLine($"\nWait for next request it will triggere at: {DateTime.UtcNow.AddMinutes(1)}");
                        Console.WriteLine($"\nFetching Projects from DB for Procerssing: {DateTime.UtcNow}");
                    }
                    
                    
                    Thread.Sleep(1000 * 30);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }

    private static async Task GetProjectFromDB()
    {
        using (ProjectService projectService = new ProjectService())
        {
            var projects = projectService.GetProjectsForAnalysisStatus();
            var newProjects = projects.Where(p => !projectList.Contains(p) && (p.AnalysisStatus == "Start"));
            foreach (var item in newProjects)
            {
                if (projectList.Where(i => i.Id == item.Id).Count() == 0)
                {
                    projectList.Enqueue(item);
                    Console.WriteLine($"\nEnqueue: {item}");
                }
            }
        }
    }
}