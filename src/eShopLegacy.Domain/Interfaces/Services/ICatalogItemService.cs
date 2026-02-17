using eShopLegacy.Domain.Entities;

namespace eShopLegacy.Domain.Interfaces.Services;

public interface ICatalogItemService
{
    Task<IEnumerable<CatalogItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CatalogItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CatalogItem> CreateAsync(CatalogItem entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, CatalogItem entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<CatalogItem> Items, long TotalCount)> GetPaginatedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
}
