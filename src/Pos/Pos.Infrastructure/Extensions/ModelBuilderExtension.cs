using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

    internal static void ConfigureEntity(this EntityTypeBuilder<Category> entity)
    {
        entity.Property(c => c.Name).HasMaxLength(100);
        entity.Property(c => c.Description).HasMaxLength(255);
    }
}