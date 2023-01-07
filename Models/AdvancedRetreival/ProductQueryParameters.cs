namespace HPSportsPlus.Models.AdvancedRetreival
{
    public class ProductQueryParameters : QueryParameters
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? Price { get; set; }

        public string Sku { get; set; } = string.Empty; // property to handle search for sku
        public string Name { get; set; } = string.Empty; // property to handle search for name of product
    }
}
