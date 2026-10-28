using System.ComponentModel.DataAnnotations;

namespace Pos.Domain.Dtos.Products;

public sealed class ProductRequestDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "SKU is required.")]
    [StringLength(50, ErrorMessage = "SKU must be at most 50 characters.")]
    public string Sku { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Barcode must be at most 100 characters.")]
    public string? Barcode { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
    [StringLength(200, ErrorMessage = "Name must be at most 200 characters.")]
    public string Name { get; set; } = string.Empty;

    public Guid? CategoryId { get; set; }

    [Range(typeof(decimal), "0", "999999999999999", ErrorMessage = "Price must be zero or greater.")]
    public decimal Price { get; set; }

    [Range(typeof(decimal), "0", "999999999999999", ErrorMessage = "Cost must be zero or greater.")]
    public decimal Cost { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Reorder level must be zero or greater.")]
    public int ReorderLevel { get; set; }
}
