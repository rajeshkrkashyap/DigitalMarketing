using Core.Shared;
using Core.Shared.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Server
{
    public class ProjectService : BaseService
    {
        public Project GetProjectById(string id)
        {
            var returnResponse = new Project();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectGetById}/?id=" + id;
                var response = client.PostAsync(url, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = response.Content.ReadAsStringAsync().Result;
                    returnResponse = JsonConvert.DeserializeObject<Project>(contentStr);
                }
            }
            return returnResponse;
        }
        public Project GetProjectId(string projectUrl)
        {
            var returnResponse = new Project();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectGetByURL}/?url=" + projectUrl;
                var response = client.PostAsync(url, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = response.Content.ReadAsStringAsync().Result;
                    returnResponse = JsonConvert.DeserializeObject<Project>(contentStr);
                }
            }
            return returnResponse;
        }
        public List<Project> GetProjectsForAnalysisStatus()
        {
            var returnResponse = new List<Project>();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.GetProjectsForAnalysisStatus}";
                var response = client.PostAsync(url, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = response.Content.ReadAsStringAsync().Result;
                    returnResponse = JsonConvert.DeserializeObject<List<Project>>(contentStr);
                }
            }
            return returnResponse;
        }
        
        public Project GetProject()
        {
            var returnResponse = new Project();
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectGetNew}";

                var response = client.PostAsync(url, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = response.Content.ReadAsStringAsync().Result;
                    returnResponse = JsonConvert.DeserializeObject<Project>(contentStr);
                }
            }
            return returnResponse;
        }

        public bool ProjectUpdateStatus(string id, string status)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectUpdateStatus}/?id=" + id + "&&status="+ status;

                var response = client.PostAsync(url, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = response.Content.ReadAsStringAsync().Result;
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }
        public bool ProjectUpdatePageCount(string id, int pageCount)
        {
            var returnResponse = false;
            using (var client = new HttpClient())
            {
                var url = $"{ApiBaseURL}{APIs.ProjectUpdatePageCount}/?id=" + id + "&&pageCount=" + pageCount;

                var response = client.PostAsync(url, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    string contentStr = response.Content.ReadAsStringAsync().Result;
                    returnResponse = JsonConvert.DeserializeObject<bool>(contentStr);
                }
            }
            return returnResponse;
        }
    }


}
