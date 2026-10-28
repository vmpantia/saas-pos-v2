using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Pos.Domain.Contracts;
using Pos.Infrastructure.Security;

namespace Pos.Api;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    // "sub" may be mapped to NameIdentifier depending on the inbound claim mapping.
    public Guid? UserId => GetGuid(JwtRegisteredClaimNames.Sub) ?? GetGuid(ClaimTypes.NameIdentifier);

    public Guid? TenantId => GetGuid(AuthClaims.TenantId);

    private Guid? GetGuid(string claimType) =>
        Guid.TryParse(Principal?.FindFirst(claimType)?.Value, out var value) ? value : null;
}
