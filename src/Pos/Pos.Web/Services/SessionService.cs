using Microsoft.AspNetCore.Components;
using Pos.Web.Models;

namespace Pos.Web.Services;

public sealed class SessionService(JwtAuthenticationStateProvider authProvider, NavigationManager navigation)
{
    public async Task SignOutAsync()
    {
        await authProvider.SignOutAsync();
        navigation.NavigateTo("login", replace: true);
    }

    // Signs the user out and returns true when the API rejected the token.
    public async Task<bool> HandleUnauthorizedAsync(ApiError? error)
    {
        if (error is not { IsUnauthorized: true }) return false;

        await authProvider.SignOutAsync();
        var returnUrl = Uri.EscapeDataString(navigation.ToBaseRelativePath(navigation.Uri));
        navigation.NavigateTo($"login?returnUrl={returnUrl}", replace: true);
        return true;
    }
}
