using System;
using System.Collections.Generic;
using CORE.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mono.TextTemplating;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext ctx;

    public ProductsController(StoreContext ctx)
    {
        this.ctx = ctx;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await ctx.Products.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await ctx.Products.FindAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        ctx.Products.Add(product);

        await ctx.SaveChangesAsync();

        var pro = GetProduct(product.Id);

        return product;
    }

    [HttpPut("{id:int}")]

    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (!ProductExists(id) || product.Id != id)
            return BadRequest("Can't update this product because id doesn't exist");

        ctx.Entry(product).State = EntityState.Modified;

        await ctx.SaveChangesAsync();

        return NoContent();
    }


    [HttpDelete("{id:int}")]

    public async Task<ActionResult> DeleteProduct(int id)
    {
        var pro = await ctx.Products.FindAsync(id);
        if (pro == null)
            return BadRequest("Can't Delete this product because id doesn't exist");

        ctx.Products.Remove(pro);

        await ctx.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return ctx.Products.Any(x => x.Id == id);
    }
}
