using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Extensions;
using Pos.Domain.Contracts.Services;
using Pos.Domain.Dtos.Products;

namespace Pos.Api.Controllers;

[ApiController]
[Authorize]
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

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ProductRequestDto request)
    {
        var result = await productService.CreateAsync(request);
        return result.ToCreatedResult(p => $"{Request.Path}/{p.Id}");
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ProductRequestDto request)
    {
        var result = await productService.UpdateAsync(id, request);
        return result.ToActionResult();
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> ActivateAsync(Guid id)
    {
        var result = await productService.SetActiveAsync(id, true);
        return result.ToActionResult();
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateAsync(Guid id)
    {
        var result = await productService.SetActiveAsync(id, false);
        return result.ToActionResult();
    }
}
