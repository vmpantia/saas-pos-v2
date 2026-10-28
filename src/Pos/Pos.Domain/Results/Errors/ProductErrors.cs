namespace Pos.Domain.Results.Errors;

public class ProductErrors
{
    public static Error NotFound(Guid id) => Error.NotFound("Product.NotFound", $"Product with the specified ID '{id}' was not found.");
    public static Error SkuAlreadyExists(string sku) => Error.Conflict("Product.SkuAlreadyExists", $"A product with the SKU '{sku}' already exists.");
    public static Error InvalidCategory(Guid id) => Error.Validation("Product.InvalidCategory", $"Category '{id}' does not exist or is inactive.");
}
