using Pos.Domain.Contracts;
using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Contracts.Services;
using Pos.Domain.Dtos.Auth;
using Pos.Domain.Results;
using Pos.Domain.Results.Errors;

namespace Pos.Application.Services;

public sealed class AuthService(ITenantRepository tenantRepository, IUserRepository userRepository, 
    IPasswordHasher passwordHasher, ITokenService tokenService) : IAuthService
{
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var tenant = await tenantRepository.GetOneByExpressionAsync(t => t.Code == request.TenantCode, cancellationToken);
        if (tenant is null) return AuthErrors.InvalidCredentials();

        var user = await userRepository.GetOneAsync(tenant, request, cancellationToken);
        if (user is null || !passwordHasher.Verify(user, request.Password)) return AuthErrors.InvalidCredentials();

        return tokenService.GenerateToken(user);
    }
}
