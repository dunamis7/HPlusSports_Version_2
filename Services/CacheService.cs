using HPSportsPlus.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;



namespace HPSportsPlus.Services;

public class CacheService
{
    private readonly ShopDbContext _context;
    private readonly IDistributedCache _cache;

    public CacheService(ShopDbContext context,IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }
    
    
    public PriceSummary ProductPriceStatistics()
    {
        IQueryable products = _context.Products;

        var minPrice = _context.Products.Min(p => p.Price);
        var maxPrice = _context.Products.Max(p => p.Price);
        var avgPrice = _context.Products.Average(p => p.Price);


        return new PriceSummary()
        {
            MinimumPrice = minPrice,
            MaximumPrice = maxPrice,
            AveragePrice = avgPrice
        };
    }


    public async Task<PriceSummary> RedisData()
    {
        var priceData = ProductPriceStatistics();
        var cachedData = await _cache.GetStringAsync("PriceDashboard");

        if (cachedData != null)
        {
            //deserialize already existing data in cache
            priceData = JsonConvert.DeserializeObject<PriceSummary>(cachedData);
        }
        else
        {
            var expireTime = DateTime.Now.AddMinutes(10);
            var totalSeconds = expireTime.Subtract(DateTime.Now).TotalSeconds;
            var serializeData = JsonConvert.SerializeObject(priceData);
            
            var entryOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(totalSeconds),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            await _cache.SetStringAsync("PriceDashboard",serializeData,entryOptions );
        }

        return priceData;
    }


}