using CodeFirstApi.Repositories;
using CodeFirstApi.Repositories.Interfaces;
using DB;
using Microsoft.Extensions.DependencyInjection;

namespace CodeFirstApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddScoped<IBeerRepository, BeerRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();

            services.AddDbContext<BarContext>();

            return services;
        }
    }
}
