using API.RequestHelpers;
using CORE.Entities;
using CORE.Interfaces;
using CORE.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IUnitOfWork unit) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts
    ([FromQuery] ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);

        return await CreatePagedResult(unit.genericRepository<Product>(),spec,specParams.PageIndex,specParams.PageSize);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await unit.genericRepository<Product>().GetByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        unit.genericRepository<Product>().Add(product);

        if (await unit.Complete())
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

        unit.genericRepository<Product>().Update(product);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem Updating the product");
    }


    [HttpDelete("{id:int}")]

    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await unit.genericRepository<Product>().GetByIdAsync(id);
        if (product == null)
            return NotFound();

        unit.genericRepository<Product>().Delete(product);

        if (await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Product was not deleted");
    }

    [HttpGet("Types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductsTypes()
    {
        var spec = new BrandListSpecification();

        return Ok(await unit.genericRepository<Product>().ListAsync(spec));
    }

    [HttpGet("brands")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetProductsBrands()
    {
        var spec = new TypeListSpecification();
        return Ok(await unit.genericRepository<Product>().ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        return unit.genericRepository<Product>().Exists(id);
    }

    
}
