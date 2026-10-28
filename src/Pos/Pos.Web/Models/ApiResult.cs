using System.Net;
using System.Text.Json.Serialization;

namespace Pos.Web.Models;

public sealed class ApiError
{
    public int Status { get; init; }
    public string Title { get; init; } = "Something went wrong.";
    public string Detail { get; init; } = "An unexpected error occurred. Please try again.";
    public string? Code { get; init; }
    public string? ErrorType { get; init; }
    public IDictionary<string, string[]>? Errors { get; init; }

    public bool IsUnauthorized => Status == (int)HttpStatusCode.Unauthorized;

    public string Message
    {
        get
        {
            if (Errors is { Count: > 0 })
                return string.Join(" ", Errors.Values.SelectMany(v => v));
            return Detail;
        }
    }
}

// Shape of the API's ProblemDetails responses (including the custom "code" and "errorType" fields).
internal sealed class ProblemDetailsPayload
{
    [JsonPropertyName("status")] public int? Status { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("detail")] public string? Detail { get; set; }
    [JsonPropertyName("code")] public string? Code { get; set; }
    [JsonPropertyName("errorType")] public string? ErrorType { get; set; }
    [JsonPropertyName("errors")] public Dictionary<string, string[]>? Errors { get; set; }
}

public sealed class ApiResult<T>
{
    public T? Value { get; private init; }
    public ApiError? Error { get; private init; }
    public bool IsSuccess => Error is null;

    public static ApiResult<T> Success(T value) => new() { Value = value };
    public static ApiResult<T> Failure(ApiError error) => new() { Error = error };
}
