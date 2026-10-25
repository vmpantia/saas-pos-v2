using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Entities;
using Pos.Infrastructure.DataAccess.Contexts;

namespace Pos.Infrastructure.DataAccess.Repositories;

public sealed class TenantRepository(PosDbContext context) : BaseRepository<Tenant>(context), ITenantRepository;