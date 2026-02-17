using eShopLegacy.Domain.Entities;

namespace eShopLegacy.Domain.Interfaces.Repositories;

public interface ICatalogTypeRepository
{
    Task<IEnumerable<CatalogType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CatalogType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CatalogType> AddAsync(CatalogType entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(CatalogType entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
