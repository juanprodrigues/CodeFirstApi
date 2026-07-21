using CodeFirstApi.Repositories;
using CodeFirstApi.Repositories.Interfaces;
using CodeFirstApi.Services;
using CodeFirstApi.Services.Interfaces;

namespace CodeFirstApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IBeerRepository, BeerRepository>();
            services.AddScoped<IBeerService, BeerService>();

            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();

            return services;
        }
    }
}