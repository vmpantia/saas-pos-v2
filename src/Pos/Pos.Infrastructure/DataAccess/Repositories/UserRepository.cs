using Microsoft.EntityFrameworkCore;
using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Dtos.Auth;
using Pos.Domain.Entities;
using Pos.Infrastructure.DataAccess.Contexts;

namespace Pos.Infrastructure.DataAccess.Repositories;

public sealed class UserRepository(PosDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetOneAsync(Tenant tenant, LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await GetAll()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.TenantId == tenant.Id && u.Username == request.Username, cancellationToken);

        return user;
    }
}
