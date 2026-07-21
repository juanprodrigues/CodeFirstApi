
using CodeFirstApi.Services;
using CodeFirstApi.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace CodeFirstApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IBeerService, BeerService>();
            services.AddScoped<IBrandService, BrandService>();

            return services;
        }
    }
}
