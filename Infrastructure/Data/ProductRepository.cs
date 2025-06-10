using System;
using System.Security.Cryptography.X509Certificates;
using CORE.Entities;
using CORE.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext ctx) : IProductRepository
{

    public void AddProduct(Product product)
    {
        ctx.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        ctx.Products.Remove(product);
    }

    public bool DoesProductExists(int id)
    {
        return ctx.Products.Any(x => x.Id == id);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await ctx.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand,string? type,string?sort)
    {
        var query = ctx.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(a => a.Brand == brand);

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(a => a.Type == type);

            query = sort switch
            {
                "priceAsc" => query.OrderBy(x => x.Price),
                "priceDesc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Name)
            };
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetProductsBrandsAsync()
    {
        return await ctx.Products.Select(x => x.Brand)
        .Distinct()
        .ToListAsync();
    }

  

    public async Task<IReadOnlyList<string>> GetProductsTypesAsync()
    {
        return await ctx.Products.Select(x => x.Type)
        .Distinct()
        .ToListAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await ctx.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        ctx.Entry(product).State = EntityState.Modified;
    }
}