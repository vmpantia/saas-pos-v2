using Pos.Domain.Dtos.Categories;

namespace Pos.Domain.Contracts.Services;

public interface ICategoryService
{
    public Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<CategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
