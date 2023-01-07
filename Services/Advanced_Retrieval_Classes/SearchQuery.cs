using HPSportsPlus.Models;
using HPSportsPlus.Models.AdvancedRetreival;
using Microsoft.AspNetCore.Mvc;

namespace HPSportsPlus.Services.Advanced_Retrieval_Classes
{
    public static class SearchQuery
    {

        public static IQueryable<Product> CustomSearch( IQueryable<Product> products, ProductQueryParameters productQuery)
        {
         
            //Looks for strings the same as what is entered in query.Search is case sensitive
            if (!string.IsNullOrEmpty(productQuery.Sku))//checks if a string is provided for Sku in query
            {
                products = products.Where(
                    p => p.Sku.ToLower().Contains(productQuery.Sku.ToLower())
                    );
            }

            ////Non-case sensitive search for name of product     
            if (!string.IsNullOrEmpty(productQuery.Name))
            {
                products = products.Where(p => p.Sku.ToLower().Contains(productQuery.Name.ToLower()) ||
                p.Name.ToLower().Contains( productQuery.Name.ToLower()));

            }



            if (productQuery.Price != null)
            {
                products = products.Where(
                    p => p.Price == productQuery.Price  
                    );

            }


            //search for range of prices
            if (productQuery.MinPrice != null) //if min price  is provided
                {
                    products = products.Where(

                        p => p.Price >= productQuery.MinPrice.Value);

                }

            if (productQuery.MaxPrice != null) // if max price is provided
            {
                products = products.Where(
                    p => p.Price <= productQuery.MaxPrice.Value);
            }

            if (productQuery.MinPrice != null && productQuery.MaxPrice != null)
            {
                products = products.Where(
                    p => p.Price >= productQuery.MinPrice.Value && p.Price <= productQuery.MaxPrice.Value
                    );
            }

            return products;

        }

    }
}
