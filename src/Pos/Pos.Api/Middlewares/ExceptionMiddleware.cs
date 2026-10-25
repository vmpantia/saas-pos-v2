using System.Net;
using Microsoft.AspNetCore.Mvc;
using Pos.Application.Exceptions;

namespace Pos.Api.Middlewares;

public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var (status, title) = ex switch
            {
                NotFoundException => (HttpStatusCode.NotFound, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };
            if (status == HttpStatusCode.InternalServerError) logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsJsonAsync(new ProblemDetails { Status = (int)status, Title = title, Detail = ex.Message });
        }
    }
}