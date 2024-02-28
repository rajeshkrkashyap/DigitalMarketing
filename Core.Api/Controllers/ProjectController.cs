using Microsoft.AspNetCore.Mvc;
using Core.Api.Models;
using Core.Shared.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;
        public ProjectController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("List")]
        public List<Project> LoadAllProjects(string userId)
        {
            return _dbContext.Projects.Where(t => t.AppUserId == userId).ToList();
        }

        [HttpPost("GetByURL")]
        public Project GetByURL(string url)
        {
            var project = _dbContext.Projects.FirstOrDefault(inst => inst.URL.Contains(url));
            return project;
        }

        [HttpPost("GetProjectsForAnalysisStatus")]
        public List<Project> GetProjectsForAnalysisStatus()
        {
            return _dbContext.Projects.Where(i => i.IsActive == true).ToList();
        }

        [HttpPost("GetById")]
        public Project GetProject(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var project = _dbContext.Projects.FirstOrDefault(inst => inst.Id == id && inst.IsActive);
                if (project!=null)
                {
                    return project;
                }
            }
            return new Project();
        }

        [HttpPost("GetNew")]
        public Project GetNewProject(string id)
        {
            return _dbContext.Projects.FirstOrDefault(inst => inst.Id == id);
        }

        [HttpPost("GetByName")]
        public Project GetProjectByName(string name, string appUserId)
        {
            return _dbContext.Projects.FirstOrDefault(inst => inst.Name == name && inst.AppUserId == appUserId);
        }
        [HttpPost("IsExist")]
        public bool GetIsExist(string name, string appUserId)
        {
            var Project = _dbContext.Projects.FirstOrDefault(inst => inst.Name == name && inst.AppUserId == appUserId);

            if (Project == null)
            {
                return false;
            }
            return true;
        }

        [HttpPost("Create")]
        public Project Create(Project Project)
        {
            if (Project != null)
            {
                var Projectdb = _dbContext.Projects.Add(new Project
                {
                    Name = Project.Name,
                    Description = Project.Description,
                    URL = Project.URL,
                    AppUserId = Project.AppUserId,
                    AnalysisStatus = "Start",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,

                    IsActive = true,
                });
                _dbContext.SaveChanges();
                return Projectdb.Entity;
            }
            return null;
        }


        [HttpPost("Update")]
        public bool Update(Project Project)
        {
            if (Project != null)
            {
                var dbProject = _dbContext.Projects.FirstOrDefault(inst => inst.Id == Project.Id);

                dbProject.Name = Project.Name;
                dbProject.Description = Project.Description;
                dbProject.Updated = DateTime.UtcNow;

                _dbContext.Projects.Update(dbProject);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        [HttpPost("ReAnalysis")]
        public bool ReAnalysis(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var dbProject = _dbContext.Projects.FirstOrDefault(inst => inst.Id == id);
                dbProject.AnalysisStatus = "Start";
                dbProject.Updated = DateTime.UtcNow;
                _dbContext.Projects.Update(dbProject);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        [HttpPost("UpdateStatus")]
        public bool UpdateStatus(string id, string status)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(status))
            {
                var dbProject = _dbContext.Projects.FirstOrDefault(inst => inst.Id == id);
                dbProject.AnalysisStatus = status;
                dbProject.Updated = DateTime.UtcNow;
                _dbContext.Projects.Update(dbProject);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        [HttpPost("Delete")]
        public bool Delete(string id)
        {
            if (id != null)
            {
                var Project = _dbContext.Projects.FirstOrDefault(inst => inst.Id == id);
                Project.IsActive = false;
                _dbContext.Projects.Update(Project);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
