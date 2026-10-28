using Pos.Domain.Dtos.Categories;
using Pos.Domain.Dtos.Products;

namespace Pos.Web.Models;

// Converts response DTOs into request DTOs so edit forms start from the current values.
public static class DtoMappings
{
    public static CategoryRequestDto ToRequest(this CategoryDto? category) => category is null
        ? new CategoryRequestDto()
        : new CategoryRequestDto { Name = category.Name, Description = category.Description };

    public static ProductRequestDto ToRequest(this ProductDto? product) => product is null
        ? new ProductRequestDto()
        : new ProductRequestDto
        {
            Sku = product.Sku,
            Barcode = product.Barcode,
            Name = product.Name,
            CategoryId = product.Category?.Id,
            Price = product.Price,
            Cost = product.Cost,
            ReorderLevel = product.ReorderLevel
        };
}
