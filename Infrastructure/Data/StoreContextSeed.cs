using System;
using System.Text.Json;
using CORE.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsycn(StoreContext ctx)
    {
        if (!ctx.Products.Any())
        {
            var ProductsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");


            var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

            if (products == null) return;

            ctx.Products.AddRange(products);

            await ctx.SaveChangesAsync();
        }

        
        if (!ctx.deliveryMethods.Any())
        {
            var DeliveryMethodData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");


            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);

            if (deliveryMethods == null) return;

            ctx.deliveryMethods.AddRange(deliveryMethods);

            await ctx.SaveChangesAsync();
        }
    }
   
}
