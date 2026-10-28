using Microsoft.AspNetCore.Mvc;
using Pos.Domain.Enums;
using Pos.Domain.Results;
using Pos.Domain.Results.Errors;

namespace Pos.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess) return new NoContentResult();
        return ToProblem(result.Error);
    }

    public static IActionResult ToActionResult<TValue>(this Result<TValue> result)
    {
        if (result.IsSuccess) return new OkObjectResult(result.Value);
        return ToProblem(result.Error);
    }

    private static ObjectResult ToProblem(Error error)
    {
        var status = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Type = $"https://httpstatuses.io/{status}",
            Title = GetTitle(error.Type),
            Detail = error.Description,
            Extensions = { ["code"] = error.Code, ["errorType"] = error.Type.ToString() }
        };

        return new ObjectResult(problem) { StatusCode = status };
    }

    private static string GetTitle(ErrorType type) => type switch
    {
        ErrorType.Validation => "One or more validation errors occurred.",
        ErrorType.NotFound => "The requested resource was not found.",
        ErrorType.Conflict => "The request conflicts with the current state of the resource.",
        ErrorType.Unauthorized => "Authentication is required.",
        ErrorType.Forbidden => "You do not have permission to perform this action.",
        _ => "An unexpected error occurred."
    };
}
