using HPSportsPlus.Models;
using System.ComponentModel.DataAnnotations;

namespace HPSportsPlus.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }


    }
}
