using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Api.Models;
using Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Twilio.TwiML.Voice;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ServiceController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("List")]
        public List<ApplicationService> LoadAllServices()
        {
            return _dbContext.ApplicationServices.ToList();
        }

        [HttpPost("GetById")]
        public ApplicationService GetService(string id)
        {
            return _dbContext.ApplicationServices.FirstOrDefault(inst => inst.Id == id);
        }
        [HttpPost("GetByName")]
        public ApplicationService GetServiceByName(string name)
        {
            return _dbContext.ApplicationServices.FirstOrDefault(inst => inst.Name == name);
        }

        [HttpPost("Create")]
        public ApplicationService Create(ApplicationService Service)
        {
            if (Service != null)
            {
                _dbContext.ApplicationServices.Add(new ApplicationService
                {
                    Name = Service.Name,
                    Description = Service.Description,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    IsActive = true,
                });
                _dbContext.SaveChanges();
                return Service;
            }
            return null;
        }

        [HttpPost("Update")]
        public bool Update(ApplicationService Service)
        {
            if (Service != null)
            {
                var dbService = _dbContext.ApplicationServices.FirstOrDefault(inst => inst.Id == Service.Id);

                dbService.Name = Service.Name;
                dbService.Description = Service.Description;
                dbService.Updated = DateTime.UtcNow;

                _dbContext.ApplicationServices.Update(dbService);
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
                var Service = _dbContext.ApplicationServices.FirstOrDefault(inst => inst.Id == id);
                Service.IsActive = false;
                _dbContext.ApplicationServices.Update(Service);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
