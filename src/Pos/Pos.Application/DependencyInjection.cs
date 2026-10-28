using Microsoft.Extensions.DependencyInjection;
using Pos.Application.Services;
using Pos.Domain.Contracts.Services;

namespace Pos.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddServices();
            return services;
        }

        private void AddServices()
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}