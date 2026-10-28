using Microsoft.EntityFrameworkCore;
using Pos.Domain.Contracts;
using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Contracts.Services;
using Pos.Domain.Dtos.Categories;
using Pos.Domain.Entities;
using Pos.Domain.Results;
using Pos.Domain.Results.Errors;

namespace Pos.Application.Services;

public sealed class CategoryService(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : ICategoryService
{
    public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await categoryRepository
            .GetAll()
            .AsNoTracking()
            .OrderBy(c => c.Name)
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

    public async Task<Result<CategoryDto>> CreateAsync(CategoryRequestDto request, CancellationToken cancellationToken = default)
    {
        if (currentUser.TenantId is not { } tenantId) return AuthErrors.MissingSession();

        var name = request.Name.Trim();
        if (await categoryRepository.AnyAsync(c => c.Name == name, cancellationToken))
            return CategoryErrors.NameAlreadyExists(name);

        var category = new Category
        {
            TenantId = tenantId,
            Name = name,
            Description = NormalizeOptional(request.Description),
            CreatedBy = Actor()
        };

        await categoryRepository.AddAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return category.Map();
    }

    public async Task<Result<CategoryDto>> UpdateAsync(Guid id, CategoryRequestDto request, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null) return CategoryErrors.NotFound(id);

        var name = request.Name.Trim();
        if (await categoryRepository.AnyAsync(c => c.Id != id && c.Name == name, cancellationToken))
            return CategoryErrors.NameAlreadyExists(name);

        category.Name = name;
        category.Description = NormalizeOptional(request.Description);
        Touch(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return category.Map();
    }

    public async Task<Result<CategoryDto>> SetActiveAsync(Guid id, bool isActive, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null) return CategoryErrors.NotFound(id);

        if (category.IsActive != isActive)
        {
            category.IsActive = isActive;
            Touch(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return category.Map();
    }

    private void Touch(Category category)
    {
        category.ModifiedAtUtc = DateTime.UtcNow;
        category.ModifiedBy = Actor();
    }

    private string Actor() => currentUser.Username ?? currentUser.UserId?.ToString() ?? "system";

    private static string? NormalizeOptional(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
