using eShopLegacy.Application.Services;
using eShopLegacy.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace eShopLegacy.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICatalogItemService, CatalogItemService>();

        return services;
    }
}
