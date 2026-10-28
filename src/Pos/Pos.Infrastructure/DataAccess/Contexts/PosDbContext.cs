using Microsoft.EntityFrameworkCore;
using Pos.Domain.Entities;
using Pos.Infrastructure.Extensions;

namespace Pos.Infrastructure.DataAccess.Contexts;

public sealed class PosDbContext(DbContextOptions<PosDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureEntityIds();
        modelBuilder.Entity<Tenant>(e => e.ConfigureEntity());
        modelBuilder.Entity<Product>(e => e.ConfigureEntity());
        modelBuilder.Entity<Category>(e => e.ConfigureEntity());
        modelBuilder.Entity<User>(e => e.ConfigureEntity());
    }
}