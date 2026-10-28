using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pos.Domain.Contracts.Repositories;
using Pos.Domain.Contracts.Services;
using Pos.Infrastructure.DataAccess.Contexts;
using Pos.Infrastructure.DataAccess.Repositories;
using Pos.Infrastructure.Security;

namespace Pos.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddDbContexts(configuration);
            services.AddRepositories();
            services.AddSecurity(configuration);
            return services;
        }

        private void AddSecurity(IConfiguration configuration)
        {
            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection(JwtSettings.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenService, JwtTokenService>();
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
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}