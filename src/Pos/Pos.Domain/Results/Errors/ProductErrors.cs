namespace Pos.Domain.Results.Errors;

public class ProductErrors
{
    public static Error NotFound(Guid id) => Error.NotFound("Product.NotFound", $"Product with the specified ID '{id}' was not found.");
}