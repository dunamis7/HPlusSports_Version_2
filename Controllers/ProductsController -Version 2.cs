using HPSportsPlus.Models;
using HPSportsPlus.Models.AdvancedRetreival;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HPSportsPlus.Controllers
{
    [ApiVersion("2.0")]
    [ApiController]
    //[Route("api/[controller]")]

    // [Route("v{v:apiVersion}/products")]
    [Route("products")]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly ShopDbContext _context;

        public ProductsV2Controller(ShopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters) // instantiating query parameters
        {
            IQueryable<Product> products = 
                _context.Products.Where(p=> p.IsAvailable==true); // returns all items as an IQueryable so we can query it

            if (queryParameters.MinPrice != null) //if min price  is provided
            {
                products = products.Where(
                    p => p.Price >= queryParameters.MinPrice.Value);

            }

            if (queryParameters.MaxPrice != null) // if max price is provided
            {
                products = products.Where(
                    p => p.Price <= queryParameters.MaxPrice.Value);
            }

            //Looks for strings the same as what is entered in query.Search is case sensitive
            if (!string.IsNullOrEmpty(queryParameters.Sku))//checks if a string is provided for Sku in query
            {
                products = products.Where(
                    p => p.Sku.ToLower().Contains(queryParameters.Sku.ToLower())
                    );
            }

            ////Non-case sensitive search for name of product     
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                if (queryParameters.Name.Length < 4)
                {
                    products = products.Where(
                    p => p.Sku.ToLower().Contains(
                        queryParameters.Name.ToLower())
                    );
                }
                else
                {
                    products = products.Where(
                    p => p.Name.ToLower().Contains(
                        queryParameters.Name.ToLower())
                    );
                }

            }

            //Non-case search for where both sku and name is provided
            //Redundant code. The last two if satements can achieve this
            if ((!string.IsNullOrEmpty(queryParameters.Sku)) && (!string.IsNullOrEmpty(queryParameters.Name)))
            {
                products = products.Where(
                    p => p.Name.ToLower().Contains(
                        queryParameters.Name.ToLower()) &&
                        p.Sku.ToLower().Contains(queryParameters.Sku.ToLower())
                    );
            }

            //Condition to do sorting
            if (!string.IsNullOrEmpty(queryParameters.Sortby))
            {
                if (typeof(Product).GetProperty(queryParameters.Sortby) != null)// checks if the keyword to sort by is a property or column for the table
                {
                    products = products.OrderByCustom(
                        queryParameters.Sortby,
                        queryParameters.SortOrder
                        );
                }
            }

            products = products
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return Ok(await products.ToListAsync());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);

        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var products = new List<Product>();

            foreach (var id in ids)
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null) return NotFound();

                products.Add(product);

            }

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();

            return Ok(products);
        }


    }

}
