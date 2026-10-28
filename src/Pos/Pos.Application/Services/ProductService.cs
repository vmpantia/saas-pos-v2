using Microsoft.EntityFrameworkCore;
using Pos.Domain.Contracts;
using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Contracts.Services;
using Pos.Domain.Dtos.Products;
using Pos.Domain.Entities;
using Pos.Domain.Results;
using Pos.Domain.Results.Errors;

namespace Pos.Application.Services;

public sealed class ProductService(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : IProductService
{
    public async Task<Result<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await productRepository
            .GetAll()
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return products.Select(p => p.Map()).ToList();
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await FindAsync(id, cancellationToken);
        if (product is null) return ProductErrors.NotFound(id);

        return product.Map();
    }

    public async Task<Result<ProductDto>> CreateAsync(ProductRequestDto request, CancellationToken cancellationToken = default)
    {
        if (currentUser.TenantId is not { } tenantId) return AuthErrors.MissingSession();

        var sku = request.Sku.Trim();
        if (await productRepository.AnyAsync(p => p.Sku == sku, cancellationToken))
            return ProductErrors.SkuAlreadyExists(sku);

        var category = await ResolveCategoryAsync(request.CategoryId, cancellationToken);
        if (request.CategoryId is { } categoryId && category is null) return ProductErrors.InvalidCategory(categoryId);

        var product = new Product
        {
            TenantId = tenantId,
            CreatedBy = Actor()
        };
        Apply(product, request, sku, category);

        await productRepository.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Map();
    }

    public async Task<Result<ProductDto>> UpdateAsync(Guid id, ProductRequestDto request, CancellationToken cancellationToken = default)
    {
        var product = await FindAsync(id, cancellationToken);
        if (product is null) return ProductErrors.NotFound(id);

        var sku = request.Sku.Trim();
        if (await productRepository.AnyAsync(p => p.Id != id && p.Sku == sku, cancellationToken))
            return ProductErrors.SkuAlreadyExists(sku);

        // Keeping a category that has since been deactivated is allowed; choosing a new one must be active.
        var category = request.CategoryId == product.CategoryId
            ? product.Category
            : await ResolveCategoryAsync(request.CategoryId, cancellationToken);
        if (request.CategoryId is { } categoryId && category is null) return ProductErrors.InvalidCategory(categoryId);

        Apply(product, request, sku, category);
        Touch(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Map();
    }

    public async Task<Result<ProductDto>> SetActiveAsync(Guid id, bool isActive, CancellationToken cancellationToken = default)
    {
        var product = await FindAsync(id, cancellationToken);
        if (product is null) return ProductErrors.NotFound(id);

        if (product.IsActive != isActive)
        {
            product.IsActive = isActive;
            Touch(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return product.Map();
    }

    private Task<Product?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        productRepository.GetAll().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    private async Task<Category?> ResolveCategoryAsync(Guid? categoryId, CancellationToken cancellationToken)
    {
        if (categoryId is not { } id) return null;
        return await categoryRepository.GetOneByExpressionAsync(c => c.Id == id && c.IsActive, cancellationToken);
    }

    private static void Apply(Product product, ProductRequestDto request, string sku, Category? category)
    {
        product.Sku = sku;
        product.Barcode = string.IsNullOrWhiteSpace(request.Barcode) ? null : request.Barcode.Trim();
        product.Name = request.Name.Trim();
        product.CategoryId = category?.Id;
        product.Category = category;
        product.Price = request.Price;
        product.Cost = request.Cost;
        product.ReorderLevel = request.ReorderLevel;
    }

    private void Touch(Product product)
    {
        product.ModifiedAtUtc = DateTime.UtcNow;
        product.ModifiedBy = Actor();
    }

    private string Actor() => currentUser.Username ?? currentUser.UserId?.ToString() ?? "system";
}
