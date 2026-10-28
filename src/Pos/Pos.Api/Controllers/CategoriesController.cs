using Microsoft.AspNetCore.Mvc;
using Pos.Domain.Contracts.Services;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var categories = await categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var category = await categoryService.GetByIdAsync(id);
        return Ok(category);
    }
}
