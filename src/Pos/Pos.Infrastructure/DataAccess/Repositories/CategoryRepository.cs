using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Entities;
using Pos.Infrastructure.DataAccess.Contexts;

namespace Pos.Infrastructure.DataAccess.Repositories;

public sealed class CategoryRepository(PosDbContext context) : BaseRepository<Category>(context), ICategoryRepository;