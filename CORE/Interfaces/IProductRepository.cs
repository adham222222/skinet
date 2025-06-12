using System;
using CORE.Entities;
namespace CORE.Interfaces;
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand,string?type,string?sort);
    Task<Product?> GetProductByIdAsync(int id);
    Task<IReadOnlyList<string>> GetProductsBrandsAsync();
    Task<IReadOnlyList<string>> GetProductsTypesAsync();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool DoesProductExists(int id);
    Task<bool> SaveChangesAsync();
}
