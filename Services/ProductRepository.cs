using HPSportsPlus.DTOs;
using HPSportsPlus.Models;
using HPSportsPlus.Models.AdvancedRetreival;
using HPSportsPlus.Services.Advanced_Retrieval_Classes;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace HPSportsPlus.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopDbContext _context;

        public ProductRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
        await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            
        }

        public async Task<Product> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
             _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task <IEnumerable<ProductDTO>> GetAll(ProductQueryParameters parameters)
        {
            IQueryable<Product> products = _context.Products;
            
            
            if (!string.IsNullOrEmpty(parameters.Name)
                || !string.IsNullOrEmpty(parameters.Sku) 
                || parameters.Price != null || parameters.MinPrice!= null 
                || parameters.MaxPrice !=null)
            {
             
                products = SearchQuery.CustomSearch(products, parameters);
            }


            // Condition to do sorting
            if (!string.IsNullOrEmpty(parameters.Sortby))
            {
                if (typeof(Product).GetProperty(parameters.Sortby) != null)// checks if the keyword to sort by is a property or column for the table
                {
                    products = products.OrderByCustom(
                        parameters.Sortby,
                        parameters.SortOrder
                    );
                }
            }
            //Pagination
            products = products
                .Skip(parameters.Size * (parameters.Page - 1))
                .Take(parameters.Size);


           var finalproducts = await  products.ProjectToType<ProductDTO>().ToListAsync();
            
            return finalproducts;
        }
       

        public async Task<ProductDTO> GetAProductAsync(int id)
        {
            var product = await _context.Products.Select(p =>
               new ProductDTO()
               {
                   Id = p.Id,
                   Name =p.Name,
                   Sku=p.Sku,
                   Description = p.Description,
                   IsAvailable = p.IsAvailable,
                   Price =p.Price
               } ).SingleOrDefaultAsync(x => x.Id == id);

            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.Id == id))
                {
                    return product;
                }
                else
                {
                    throw;
                }
            }

            return product;
        }
    }
}

