using Microsoft.EntityFrameworkCore;
using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Contracts.Services;
using Pos.Domain.Dtos.Products;
using Pos.Domain.Results;
using Pos.Domain.Results.Errors;

namespace Pos.Application.Services;

public sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<Result<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await productRepository
            .GetAll()
            .Select(p => p.Map())
            .ToListAsync(cancellationToken);

        return products;
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null) return ProductErrors.NotFound(id);

        return product.Map();
    }
}