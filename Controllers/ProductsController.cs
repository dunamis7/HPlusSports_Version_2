using HPSportsPlus.Models;
using HPSportsPlus.Models.AdvancedRetreival;
using Microsoft.AspNetCore.Mvc;
using HPSportsPlus.Services;
namespace HPSportsPlus.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    //[Route("api/[controller]")]

    //[Route("v{v:apiVersion}/products")] url versioning
    [Route("products")]
    public class ProductsV1Controller : ControllerBase
    {
        private readonly IProductRepository _service;
        private readonly CacheService _cache;

        public ProductsV1Controller(IProductRepository service, CacheService cache)
        {
            _service = service;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductQueryParameters queryParameters) // instantiating query parameters
        {
            var products =  await _service.GetAll(queryParameters);

            return Ok(products);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _service.GetAProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody]Product product)
        {
            await _service.AddProductAsync(product);

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            await _service.UpdateProductAsync(id, product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task <ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _service.GetAProductAsync(id);
            if(product == null) return NotFound();

             await _service.DeleteProductAsync(id);

            return Ok(product);

        }

        [HttpGet]
        [Route("redis")]
        public async Task<ActionResult> PriceSummary()
        {
            var summary = await _cache.RedisData();
            return Ok(summary);
        }


    }




 

}
