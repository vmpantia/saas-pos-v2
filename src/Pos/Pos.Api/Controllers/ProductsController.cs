using Microsoft.AspNetCore.Mvc;
using Pos.Domain.Contracts.Services;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var products = await productService.GetAllAsync();
        return Ok(products);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var product = await productService.GetByIdAsync(id);
        return Ok(product);
    }
    
}