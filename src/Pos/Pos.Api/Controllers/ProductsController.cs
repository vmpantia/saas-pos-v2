using Microsoft.AspNetCore.Mvc;
using Pos.Api.Extensions;
using Pos.Domain.Contracts.Services;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await productService.GetAllAsync();
        return result.ToActionResult();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await productService.GetByIdAsync(id);
        return result.ToActionResult();
    }
    
}