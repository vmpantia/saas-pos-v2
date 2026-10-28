using Pos.Domain.Dtos.Auth;
using Pos.Domain.Entities;

namespace Pos.Domain.Contracts.Services;

public interface ITokenService
{
    LoginResponseDto GenerateToken(User user);
}
