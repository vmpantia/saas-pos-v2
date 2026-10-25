using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Entities;
using Pos.Infrastructure.DataAccess.Contexts;

namespace Pos.Infrastructure.DataAccess.Repositories;

public abstract class BaseRepository<TEntity>(PosDbContext context) : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    public IQueryable<TEntity> GetAll()
    {
        return context.Set<TEntity>().AsQueryable();
    }
    
    public Task<List<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>().Where(expression ?? (_ => true)).ToListAsync(cancellationToken);;
    }

    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task<TEntity?> GetOneByExpressionAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>().FirstOrDefaultAsync(expression ?? (_ => true), cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return context.Set<TEntity>().AnyAsync(expression, cancellationToken);
    }
    
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }
    
    public void Remove(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
    }
}