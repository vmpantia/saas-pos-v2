using Pos.Domain.Contracts;
using Pos.Infrastructure.DataAccess.Contexts;

namespace Pos.Infrastructure.DataAccess;

public sealed class UnitOfWork(PosDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
