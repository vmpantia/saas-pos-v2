using Pos.Domain.Dtos.Auth;
using Pos.Domain.Dtos.Categories;
using Pos.Domain.Dtos.Products;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Pos.Web.Models;

namespace Pos.Web.Services;

public sealed class JwtAuthenticationStateProvider(ProtectedSessionStorage storage) : AuthenticationStateProvider
{
    private const string StorageKey = "pos.auth";
    private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    private LoginResponseDto? _session;
    private bool _loaded;

    public async Task<string?> GetAccessTokenAsync()
    {
        await EnsureLoadedAsync();
        return IsValid(_session) ? _session!.AccessToken : null;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await EnsureLoadedAsync();
        return IsValid(_session) ? new AuthenticationState(BuildPrincipal(_session!.AccessToken)) : Anonymous;
    }

    public async Task SignInAsync(LoginResponseDto response)
    {
        _session = response;
        _loaded = true;
        await storage.SetAsync(StorageKey, response);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task SignOutAsync()
    {
        _session = null;
        _loaded = true;
        await storage.DeleteAsync(StorageKey);
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }

    private async Task EnsureLoadedAsync()
    {
        if (_loaded) return;

        try
        {
            var result = await storage.GetAsync<LoginResponseDto>(StorageKey);
            _session = result.Success ? result.Value : null;
            _loaded = true;
        }
        catch (InvalidOperationException)
        {
            // JS interop isn't available yet (e.g. during prerendering); treat as signed out and retry later.
            _session = null;
        }
        catch (System.Security.Cryptography.CryptographicException)
        {
            // Stored value can't be decrypted (e.g. data-protection keys changed); discard it.
            _session = null;
            _loaded = true;
        }
    }

    private static bool IsValid(LoginResponseDto? session) =>
        session is { AccessToken.Length: > 0 } && session.ExpiresAtUtc > DateTime.UtcNow;

    private static ClaimsPrincipal BuildPrincipal(string token)
    {
        var claims = new List<Claim>();
        var parts = token.Split('.');
        if (parts.Length == 3)
        {
            using var json = JsonDocument.Parse(DecodeBase64Url(parts[1]));
            foreach (var property in json.RootElement.EnumerateObject())
            {
                if (property.Value.ValueKind is JsonValueKind.String or JsonValueKind.Number)
                    claims.Add(new Claim(property.Name, property.Value.ToString()));
            }
        }

        var identity = new ClaimsIdentity(claims, "jwt", nameType: "unique_name", roleType: "role");
        return new ClaimsPrincipal(identity);
    }

    private static byte[] DecodeBase64Url(string value)
    {
        var padded = value.Replace('-', '+').Replace('_', '/');
        padded = padded.PadRight(padded.Length + (4 - padded.Length % 4) % 4, '=');
        return Convert.FromBase64String(padded);
    }
}
