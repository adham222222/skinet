using API.RequestHelpers;
using CORE.Entities;
using CORE.Interfaces;
using CORE.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts
    ([FromQuery] ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);

        return await CreatePagedResult(repo,spec,specParams.PageIndex,specParams.PageSize);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);

        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("Problem creating a product");
    }

    [HttpPut("{id:int}")]

    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {

        if (!ProductExists(id) || product.Id != id)
            return BadRequest("The product doesn't exist");

        repo.Update(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem Updating the product");
    }


    [HttpDelete("{id:int}")]

    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null)
            return NotFound();



        repo.Delete(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Product was not deleted");
    }

    [HttpGet("Types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductsTypes()
    {
        var spec = new BrandListSpecification();

        return Ok(await repo.ListAsync(spec));
    }

    [HttpGet("brands")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetProductsBrands()
    {
        var spec = new TypeListSpecification();
        return Ok(await repo.ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        return repo.Exists(id);
    }

    
}
