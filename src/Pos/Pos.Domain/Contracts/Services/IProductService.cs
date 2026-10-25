using Pos.Domain.Dtos.Products;

namespace Pos.Domain.Contracts.Services;

public interface IProductService
{
    public Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}