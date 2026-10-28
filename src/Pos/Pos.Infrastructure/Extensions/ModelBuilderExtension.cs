using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pos.Domain.Contracts.Entities;
using Pos.Domain.Entities;

namespace Pos.Infrastructure.Extensions;

internal static class ModelBuilderExtension
{
    internal static void ConfigureEntityIds(this ModelBuilder modelBuilder)
    {
        var types = modelBuilder.Model
            .GetEntityTypes()
            .Select(e => e.ClrType)
            .Where(t => typeof(BaseEntity).IsAssignableFrom(t));
        
        foreach (var type in types)
            modelBuilder.Entity(type)
                .Property(nameof(BaseEntity.Id))
                .ValueGeneratedNever();
    }

    internal static void ConfigureEntity(this EntityTypeBuilder<Tenant> entity)
    {
        entity.HasIndex(t => t.Code).IsUnique();
    }

    internal static void ConfigureEntity(this EntityTypeBuilder<Product> entity)
    {
        entity.Property(p => p.Sku).HasMaxLength(50);
        entity.Property(p => p.Name).HasMaxLength(200);
        entity.Property(p => p.Price).HasPrecision(18, 2);
        entity.Property(p => p.Cost).HasPrecision(18, 2);
        entity.HasIndex(p => new { p.TenantId, p.Sku }).IsUnique();
        entity.HasIndex(p => new { p.TenantId, p.Barcode });
        
        entity.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired(false);
    }

    internal static void ConfigureEntity(this EntityTypeBuilder<User> entity)
    {
        entity.Property(u => u.Username).HasMaxLength(100);
        entity.Property(u => u.Email).HasMaxLength(256);
        entity.Property(u => u.PasswordHash).HasMaxLength(512);
        entity.Property(u => u.FirstName).HasMaxLength(100);
        entity.Property(u => u.MiddleName).HasMaxLength(100);
        entity.Property(u => u.LastName).HasMaxLength(100);
        entity.HasIndex(u => new { u.TenantId, u.Username }).IsUnique();
        entity.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();
    }

    internal static void ConfigureEntity(this EntityTypeBuilder<Category> entity)
    {
        entity.Property(c => c.Name).HasMaxLength(100);
        entity.Property(c => c.Description).HasMaxLength(255);
    }
}