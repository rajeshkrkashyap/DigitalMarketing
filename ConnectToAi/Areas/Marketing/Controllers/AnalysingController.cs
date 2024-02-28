using ConnectToAi.Areas.Marketing.Models;
using ConnectToAi.Services;
using Core.Shared;
using Core.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace ConnectToAi.Areas.Marketing.Controllers
{
    public class AnalysingController : BaseController
    {
        public AnalysingController(ConfigService configService) : base(configService)
        {

        }
        public IActionResult Index()
        {
            return View(new ProjectViewModel());
        }
        public IActionResult AddProject(ProjectViewModel projectViewModel)
        {

            Request.Cookies.TryGetValue("ConnectToAi_DigitalMarketing_AuthToken", out string? cookieValue);
            if (cookieValue != null)
            {
                var userDetail = JsonConvert.DeserializeObject<UserDetail?>(cookieValue);

                using (ProjectService projectService = new(_configService))
                {
                    var addProject = new Project
                    {
                        Name = projectViewModel.Name,
                        Description = projectViewModel.Description,
                        URL = projectViewModel.URL,
                        Created = DateTime.UtcNow,
                        Updated = DateTime.UtcNow,
                        IsActive = true,
                        AnalysisStatus = "Start",
                        AppUserId = userDetail.UserID
                    };

                    var project = projectService.CreateAsync(addProject).Result;

                    if (!string.IsNullOrEmpty(project.URL))
                    {
                        SendProjectForAnalysis(project.Id);
                    }
                }
            }
            return View();
        }

        private void SendProjectForAnalysis(string id)
        {
            TcpClient client = new();
            try
            {
                client.Connect("127.0.0.1", 12345); //Connect to server at IP address 127.0.0.1 (localhost) and port 12345
                NetworkStream stream = client.GetStream();
                string message = id;    //projectId //"https://royallvastramm.com" ;
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received from server: {response}");
                stream.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }


    }
}
