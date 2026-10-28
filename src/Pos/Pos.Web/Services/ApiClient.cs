using Pos.Domain.Dtos.Auth;
using Pos.Domain.Dtos.Categories;
using Pos.Domain.Dtos.Products;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Pos.Web.Models;

namespace Pos.Web.Services;

public sealed class ApiClient(HttpClient http, JwtAuthenticationStateProvider authProvider)
{
    public Task<ApiResult<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default) =>
        SendAsync<LoginResponseDto>(HttpMethod.Post, "api/auth/login", request, authenticated: false, cancellationToken);

    public Task<ApiResult<List<ProductDto>>> GetProductsAsync(CancellationToken cancellationToken = default) =>
        SendAsync<List<ProductDto>>(HttpMethod.Get, "api/products", null, authenticated: true, cancellationToken);

    public Task<ApiResult<List<CategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken = default) =>
        SendAsync<List<CategoryDto>>(HttpMethod.Get, "api/categories", null, authenticated: true, cancellationToken);

    public Task<ApiResult<ProductDto>> GetProductAsync(Guid id, CancellationToken cancellationToken = default) =>
        SendAsync<ProductDto>(HttpMethod.Get, $"api/products/{id}", null, authenticated: true, cancellationToken);

    public Task<ApiResult<ProductDto>> CreateProductAsync(ProductRequestDto form, CancellationToken cancellationToken = default) =>
        SendAsync<ProductDto>(HttpMethod.Post, "api/products", form, authenticated: true, cancellationToken);

    public Task<ApiResult<ProductDto>> UpdateProductAsync(Guid id, ProductRequestDto form, CancellationToken cancellationToken = default) =>
        SendAsync<ProductDto>(HttpMethod.Put, $"api/products/{id}", form, authenticated: true, cancellationToken);

    public Task<ApiResult<ProductDto>> SetProductActiveAsync(Guid id, bool isActive, CancellationToken cancellationToken = default) =>
        SendAsync<ProductDto>(HttpMethod.Patch, $"api/products/{id}/{(isActive ? "activate" : "deactivate")}", null, authenticated: true, cancellationToken);

    public Task<ApiResult<CategoryDto>> GetCategoryAsync(Guid id, CancellationToken cancellationToken = default) =>
        SendAsync<CategoryDto>(HttpMethod.Get, $"api/categories/{id}", null, authenticated: true, cancellationToken);

    public Task<ApiResult<CategoryDto>> CreateCategoryAsync(CategoryRequestDto form, CancellationToken cancellationToken = default) =>
        SendAsync<CategoryDto>(HttpMethod.Post, "api/categories", form, authenticated: true, cancellationToken);

    public Task<ApiResult<CategoryDto>> UpdateCategoryAsync(Guid id, CategoryRequestDto form, CancellationToken cancellationToken = default) =>
        SendAsync<CategoryDto>(HttpMethod.Put, $"api/categories/{id}", form, authenticated: true, cancellationToken);

    public Task<ApiResult<CategoryDto>> SetCategoryActiveAsync(Guid id, bool isActive, CancellationToken cancellationToken = default) =>
        SendAsync<CategoryDto>(HttpMethod.Patch, $"api/categories/{id}/{(isActive ? "activate" : "deactivate")}", null, authenticated: true, cancellationToken);

    private async Task<ApiResult<T>> SendAsync<T>(HttpMethod method, string url, object? body, bool authenticated, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, url);
        if (body is not null) request.Content = JsonContent.Create(body);

        if (authenticated)
        {
            var token = await authProvider.GetAccessTokenAsync();
            if (token is null) return ApiResult<T>.Failure(SessionExpired());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        try
        {
            using var response = await http.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var value = await response.Content.ReadFromJsonAsync<T>(cancellationToken);
                return value is null
                    ? ApiResult<T>.Failure(new ApiError { Status = (int)response.StatusCode, Title = "Empty response", Detail = "The server returned an empty response." })
                    : ApiResult<T>.Success(value);
            }

            return ApiResult<T>.Failure(await ReadErrorAsync(response, cancellationToken));
        }
        catch (HttpRequestException)
        {
            return ApiResult<T>.Failure(new ApiError
            {
                Status = 0,
                Title = "Unable to reach the server.",
                Detail = "The POS API can't be reached right now. Check your connection and try again.",
                Code = "Client.Unreachable"
            });
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return ApiResult<T>.Failure(new ApiError
            {
                Status = 0,
                Title = "The request timed out.",
                Detail = "The server took too long to respond. Please try again.",
                Code = "Client.Timeout"
            });
        }
    }

    private static async Task<ApiError> ReadErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        ProblemDetailsPayload? problem = null;
        try
        {
            problem = await response.Content.ReadFromJsonAsync<ProblemDetailsPayload>(cancellationToken);
        }
        catch (Exception ex) when (ex is System.Text.Json.JsonException or NotSupportedException)
        {
            // Non-JSON body; fall back to a generic message below.
        }

        var status = problem?.Status ?? (int)response.StatusCode;
        return new ApiError
        {
            Status = status,
            Title = problem?.Title ?? response.ReasonPhrase ?? "Request failed.",
            Detail = problem?.Detail ?? problem?.Title ?? DefaultMessage((HttpStatusCode)status),
            Code = problem?.Code,
            ErrorType = problem?.ErrorType,
            Errors = problem?.Errors
        };
    }

    private static ApiError SessionExpired() => new()
    {
        Status = (int)HttpStatusCode.Unauthorized,
        Title = "Authentication is required.",
        Detail = "Your session has expired. Please sign in again.",
        Code = "Auth.Unauthorized"
    };

    private static string DefaultMessage(HttpStatusCode status) => status switch
    {
        HttpStatusCode.Unauthorized => "Your session has expired. Please sign in again.",
        HttpStatusCode.Forbidden => "You don't have permission to do that.",
        HttpStatusCode.NotFound => "The requested resource was not found.",
        _ => "An unexpected error occurred. Please try again."
    };
}
