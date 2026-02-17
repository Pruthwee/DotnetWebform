using eShopLegacy.Domain.Entities;
using eShopLegacy.Domain.Interfaces.Repositories;
using eShopLegacy.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace eShopLegacy.Application.Services;

public class CatalogItemService : ICatalogItemService
{
    private readonly ICatalogItemRepository _repository;
    private readonly ILogger<CatalogItemService> _logger;

    public CatalogItemService(ICatalogItemRepository repository, ILogger<CatalogItemService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<CatalogItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all catalog items");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all catalog items");
            throw;
        }
    }

    public async Task<CatalogItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting catalog item with id {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting catalog item with id {Id}", id);
            throw;
        }
    }

    public async Task<CatalogItem> CreateAsync(CatalogItem entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new catalog item: {Name}", entity.Name);
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            return await _repository.AddAsync(entity, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating catalog item: {Name}", entity.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, CatalogItem entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating catalog item with id {Id}", id);
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Catalog item with id {id} not found");
            }

            entity.Id = id;
            entity.ModifiedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(entity, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating catalog item with id {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting catalog item with id {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting catalog item with id {Id}", id);
            throw;
        }
    }

    public async Task<(IEnumerable<CatalogItem> Items, long TotalCount)> GetPaginatedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting paginated catalog items: page {PageIndex}, size {PageSize}", pageIndex, pageSize);
            return await _repository.GetPaginatedAsync(pageIndex, pageSize, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated catalog items");
            throw;
        }
    }
}
