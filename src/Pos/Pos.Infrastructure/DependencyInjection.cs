using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pos.Domain.Contracts.Repositories;
using Pos.Infrastructure.DataAccess.Contexts;
using Pos.Infrastructure.DataAccess.Repositories;

namespace Pos.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddDbContexts(configuration);
            services.AddRepositories();
            return services;
        }

        private void AddDbContexts(IConfiguration configuration)
        {
            services.AddDbContext<PosDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MigrationDb"));
            });
        }

        private void AddRepositories()
        {
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}