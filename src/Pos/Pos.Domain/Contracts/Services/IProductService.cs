using Pos.Domain.Dtos.Products;
using Pos.Domain.Results;

namespace Pos.Domain.Contracts.Services;

public interface IProductService
{
    public Task<Result<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<Result<ProductDto>> CreateAsync(ProductRequestDto request, CancellationToken cancellationToken = default);
    public Task<Result<ProductDto>> UpdateAsync(Guid id, ProductRequestDto request, CancellationToken cancellationToken = default);
    public Task<Result<ProductDto>> SetActiveAsync(Guid id, bool isActive, CancellationToken cancellationToken = default);
}
