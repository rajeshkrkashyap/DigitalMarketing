using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Api.Models;
using Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;

        public ProductsController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product productToCreate)
        {
            productToCreate.Id = Guid.NewGuid().ToString();

            await _appDbContext.AddAsync(productToCreate);

            await _appDbContext.SaveChangesAsync();

            return Ok(productToCreate);
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _appDbContext.Products.ToListAsync();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Product updatedProduct)
        {
            _appDbContext.Update(updatedProduct);

            await _appDbContext.SaveChangesAsync();

            return Ok(updatedProduct);
        }

        [HttpDelete]
        [Route("{productToDeleteId}")]
        public async Task<IActionResult> Update(string productToDeleteId)
        {
            var productToDelete = await _appDbContext.Products.FindAsync(productToDeleteId);

            _appDbContext.Remove(productToDelete);

            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
