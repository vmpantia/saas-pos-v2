using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pos.Domain.Contracts;
using Pos.Domain.Entities;
using Pos.Infrastructure.DataAccess.Contexts;

namespace Pos.Infrastructure.DataAccess.Seeding;

public static class DevelopmentDataSeeder
{
    private const string TenantCode = "demo";
    private const string Username = "admin";
    private const string Password = "Admin123!";

    // Development only: applies migrations and inserts demo data when no tenant exists.
    public static async Task SeedAsync(IServiceProvider services, CancellationToken ct = default)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<PosDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await db.Database.MigrateAsync(ct);

        if (await db.Tenants.AnyAsync(ct))
            return;

        var tenant = new Tenant { Name = "Demo Store", Code = TenantCode };
        db.Tenants.Add(tenant);

        var user = new User
        {
            TenantId = tenant.Id,
            Username = Username,
            Email = "admin@demo.local",
            FirstName = "Demo",
            LastName = "Admin",
            PasswordHash = string.Empty
        };
        user.PasswordHash = hasher.Hash(user, Password);
        db.Users.Add(user);

        var beverages = NewCategory(tenant.Id, "Beverages", "Drinks and refreshments");
        var snacks = NewCategory(tenant.Id, "Snacks", "Chips, biscuits and sweets");
        var household = NewCategory(tenant.Id, "Household", "Everyday household items");
        db.Categories.AddRange(beverages, snacks, household);

        db.Products.AddRange(
            NewProduct(tenant.Id, "BEV-001", "Bottled Water 500ml", beverages.Id, 20m, 12m, 50),
            NewProduct(tenant.Id, "BEV-002", "Iced Tea 350ml", beverages.Id, 35m, 22m, 30),
            NewProduct(tenant.Id, "SNK-001", "Potato Chips 60g", snacks.Id, 45m, 30m, 20),
            NewProduct(tenant.Id, "SNK-002", "Chocolate Bar", snacks.Id, 30m, 18m, 25),
            NewProduct(tenant.Id, "HSE-001", "Dishwashing Liquid 500ml", household.Id, 85m, 60m, 10),
            NewProduct(tenant.Id, "MSC-001", "Uncategorized Item", null, 10m, 5m, 5));

        await db.SaveChangesAsync(ct);
    }

    private static Category NewCategory(Guid tenantId, string name, string description) => new()
    {
        TenantId = tenantId,
        Name = name,
        Description = description,
        CreatedBy = Username
    };

    private static Product NewProduct(Guid tenantId, string sku, string name, Guid? categoryId,
        decimal price, decimal cost, int reorderLevel) => new()
    {
        TenantId = tenantId,
        Sku = sku,
        Name = name,
        CategoryId = categoryId,
        Price = price,
        Cost = cost,
        ReorderLevel = reorderLevel,
        CreatedBy = Username
    };
}
