using Pos.Domain.Dtos.Categories;

namespace Pos.Domain.Dtos.Products;

public sealed class ProductDto
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryLiteDto? Category { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? ModifiedAtUtc { get; set; }
    public string? ModifiedBy { get; set; }
}
