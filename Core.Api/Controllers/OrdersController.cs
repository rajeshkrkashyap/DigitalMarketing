using Microsoft.AspNetCore.Mvc;
using Core.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Core.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;

        public OrdersController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order OrdersToCreate)
        {
            OrdersToCreate.Id = Guid.NewGuid().ToString();

            await _appDbContext.AddAsync(OrdersToCreate);

            await _appDbContext.SaveChangesAsync();

            return Ok(OrdersToCreate);
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _appDbContext.Orders.ToListAsync();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Order updatedOrders)
        {
            _appDbContext.Update(updatedOrders);

            await _appDbContext.SaveChangesAsync();

            return Ok(updatedOrders);
        }

        [HttpDelete]
        [Route("{OrdersToDeleteId}")]
        public async Task<IActionResult> Update(string OrdersToDeleteId)
        {
            var OrdersToDelete = await _appDbContext.Orders.FindAsync(OrdersToDeleteId);

            _appDbContext.Remove(OrdersToDelete);

            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
