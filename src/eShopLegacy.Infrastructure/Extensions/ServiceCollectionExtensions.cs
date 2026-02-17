using eShopLegacy.Domain.Interfaces.Repositories;
using eShopLegacy.Infrastructure.Data;
using eShopLegacy.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShopLegacy.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CatalogDbContext")));

        services.AddScoped<ICatalogItemRepository, CatalogItemRepository>();
        services.AddScoped<ICatalogBrandRepository, CatalogBrandRepository>();
        services.AddScoped<ICatalogTypeRepository, CatalogTypeRepository>();

        return services;
    }
}
