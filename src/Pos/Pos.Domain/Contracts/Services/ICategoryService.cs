using Pos.Domain.Dtos.Categories;
using Pos.Domain.Results;

namespace Pos.Domain.Contracts.Services;

public interface ICategoryService
{
    public Task<Result<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Result<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<Result<CategoryDto>> CreateAsync(CategoryRequestDto request, CancellationToken cancellationToken = default);
    public Task<Result<CategoryDto>> UpdateAsync(Guid id, CategoryRequestDto request, CancellationToken cancellationToken = default);
    public Task<Result<CategoryDto>> SetActiveAsync(Guid id, bool isActive, CancellationToken cancellationToken = default);
}
