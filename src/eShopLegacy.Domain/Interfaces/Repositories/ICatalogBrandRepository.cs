using eShopLegacy.Domain.Entities;

namespace eShopLegacy.Domain.Interfaces.Repositories;

public interface ICatalogBrandRepository
{
    Task<IEnumerable<CatalogBrand>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CatalogBrand?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CatalogBrand> AddAsync(CatalogBrand entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(CatalogBrand entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
