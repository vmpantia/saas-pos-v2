namespace Pos.Domain.Results.Errors;

public class CategoryErrors
{
    public static Error NotFound(Guid id) => Error.NotFound("Category.NotFound", $"Category with the specified ID '{id}' was not found.");
    public static Error NameAlreadyExists(string name) => Error.Conflict("Category.NameAlreadyExists", $"A category named '{name}' already exists.");
}
