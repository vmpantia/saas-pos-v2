using System.Linq.Expressions;
using Pos.Domain.Contracts.Entities;

namespace Pos.Domain.Contracts.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> GetAll();
    Task<List<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetOneByExpressionAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Remove(TEntity entity);
}