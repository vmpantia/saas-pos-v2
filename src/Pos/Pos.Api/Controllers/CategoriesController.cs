using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Extensions;
using Pos.Domain.Contracts.Services;
using Pos.Domain.Dtos.Categories;

namespace Pos.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await categoryService.GetAllAsync();
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await categoryService.GetByIdAsync(id);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CategoryRequestDto request)
    {
        var result = await categoryService.CreateAsync(request);
        return result.ToCreatedResult(c => $"{Request.Path}/{c.Id}");
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CategoryRequestDto request)
    {
        var result = await categoryService.UpdateAsync(id, request);
        return result.ToActionResult();
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> ActivateAsync(Guid id)
    {
        var result = await categoryService.SetActiveAsync(id, true);
        return result.ToActionResult();
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateAsync(Guid id)
    {
        var result = await categoryService.SetActiveAsync(id, false);
        return result.ToActionResult();
    }
}
