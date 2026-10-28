using System.ComponentModel.DataAnnotations;

namespace Pos.Domain.Dtos.Categories;

public sealed class CategoryRequestDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Description must be at most 255 characters.")]
    public string? Description { get; set; }
}
