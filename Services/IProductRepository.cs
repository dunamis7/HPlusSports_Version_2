using HPSportsPlus.DTOs;
using HPSportsPlus.Models;
using HPSportsPlus.Models.AdvancedRetreival;

namespace HPSportsPlus.Services
{
    public interface IProductRepository
    {
        Task<ProductDTO> GetAProductAsync(int id);

        Task<IEnumerable<ProductDTO>> GetAll(ProductQueryParameters parameters);

        Task AddProductAsync(Product product);  

        Task <Product> DeleteProductAsync(int id); 
        
        Task<Product> UpdateProductAsync(int id, Product product);
        
    }
}
