using Pos.Domain.Contracts;
using Pos.Domain.Contracts.Entities;
using Pos.Domain.Dtos.Categories;
using Pos.Domain.Dtos.Products;

namespace Pos.Domain.Entities;

public sealed class Product : TenantEntity, IMappable<ProductDto>
{
    public string Sku { get; set; } = null!;
    public string? Barcode { get; set; }
    public string Name { get; set; } = null!;
    public Guid? CategoryId { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public int ReorderLevel { get; set; }
    
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = null!;
    public DateTime? ModifiedAtUtc { get; set; }
    public string? ModifiedBy { get; set; }
    
    public Category? Category { get; set; }
    
    public ProductDto Map() => new()
    {
        Id = Id,
        Sku = Sku,
        Barcode = Barcode,
        Name = Name,
        Category = Category is null
            ? null
            : new CategoryLiteDto
            {
                Id = Category.Id,
                Name = Category.Name,
                Description = Category.Description
            },
        Price = Price,
        Cost = Cost,
        ReorderLevel = ReorderLevel,
        IsActive = IsActive,
        CreatedAtUtc = CreatedAtUtc,
        CreatedBy = CreatedBy,
        ModifiedAtUtc = ModifiedAtUtc,
        ModifiedBy = ModifiedBy
    };
}