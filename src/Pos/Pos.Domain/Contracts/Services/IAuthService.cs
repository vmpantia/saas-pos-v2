using Pos.Domain.Dtos.Auth;
using Pos.Domain.Results;

namespace Pos.Domain.Contracts.Services;

public interface IAuthService
{
    public Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}
