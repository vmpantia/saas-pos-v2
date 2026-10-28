using Microsoft.EntityFrameworkCore;
using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Contracts.Services;
using Pos.Domain.Dtos.Categories;
using Pos.Domain.Results;
using Pos.Domain.Results.Errors;

namespace Pos.Application.Services;

public sealed class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await categoryRepository
            .GetAll()
            .Select(c => c.Map())
            .ToListAsync(cancellationToken);

        return categories;
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null) return CategoryErrors.NotFound(id);

        return category.Map();
    }
}
