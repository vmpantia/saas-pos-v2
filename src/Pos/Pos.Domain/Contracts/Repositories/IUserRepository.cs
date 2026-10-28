using Pos.Domain.Dtos.Auth;
using Pos.Domain.Entities;

namespace Pos.Domain.Contracts.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetOneAsync(Tenant tenant, LoginRequestDto request, CancellationToken cancellationToken = default);
}
