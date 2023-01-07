using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace HPSportsPlus.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

    }
}
