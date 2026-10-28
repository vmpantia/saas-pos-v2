using Microsoft.EntityFrameworkCore;
using Pos.Application.Exceptions;
using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Contracts.Services;
using Pos.Domain.Dtos.Categories;

namespace Pos.Application.Services;

public sealed class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await categoryRepository
            .GetAll()
            .Select(c => c.Map())
            .ToListAsync(cancellationToken);

        return categories;
    }

    public async Task<CategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null) throw new NotFoundException($"Category with the specified ID '{id}' was not found.");

        return category.Map();
    }
}
