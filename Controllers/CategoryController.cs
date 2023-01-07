using HPSportsPlus.DTOs;
using HPSportsPlus.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPSportsPlus.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public CategoryController(ShopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories.Select(c =>
            new CategoryDTO()
            {
                id =c.Id,
                name= c.Name,
            }
            ));
        }

        [HttpGet("{id}")]
        [Route("/categories/{id}/products")]
        public async Task<ActionResult> GetCategories(int id)
        {
            var products = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
            return Ok(products.Adapt<ProductDTO>());
        }
    }
}


