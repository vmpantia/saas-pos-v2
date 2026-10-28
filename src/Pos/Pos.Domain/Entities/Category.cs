using Pos.Domain.Contracts;
using Pos.Domain.Contracts.Entities;
using Pos.Domain.Dtos.Categories;

namespace Pos.Domain.Entities;

public sealed class Category : TenantEntity, IMappable<CategoryDto>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = null!;
    public DateTime? ModifiedAtUtc { get; set; }
    public string? ModifiedBy { get; set; }
    
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public CategoryDto Map() => new()
    {
        Id = Id,
        Name = Name,
        Description = Description,
        IsActive = IsActive,
        CreatedAtUtc = CreatedAtUtc,
        CreatedBy = CreatedBy,
        ModifiedAtUtc = ModifiedAtUtc,
        ModifiedBy = ModifiedBy
    };
}