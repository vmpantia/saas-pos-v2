using Microsoft.EntityFrameworkCore;
using Pos.Application.Exceptions;
using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Contracts.Services;
using Pos.Domain.Dtos.Products;

namespace Pos.Application.Services;

public sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await productRepository
            .GetAll()
            .Select(p => p.Map())
            .ToListAsync(cancellationToken);

        return products;
    }

    public async Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null) throw new NotFoundException($"Product with the specified ID '{id}' was not found.");

        return product.Map();
    }
}