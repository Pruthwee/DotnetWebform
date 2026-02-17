using eShopLegacy.Domain.Entities;
using eShopLegacy.Domain.Interfaces.Repositories;
using eShopLegacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eShopLegacy.Infrastructure.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly CatalogDbContext _context;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(CatalogDbContext context, ILogger<CatalogItemRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<CatalogItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.CatalogItems
                .Include(c => c.CatalogBrand)
                .Include(c => c.CatalogType)
                .Where(c => c.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all catalog items");
            throw;
        }
    }

    public async Task<CatalogItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.CatalogItems
                .Include(c => c.CatalogBrand)
                .Include(c => c.CatalogType)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving catalog item with id {Id}", id);
            throw;
        }
    }

    public async Task<CatalogItem> AddAsync(CatalogItem entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.CatalogItems.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding catalog item");
            throw;
        }
    }

    public async Task UpdateAsync(CatalogItem entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.CatalogItems.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating catalog item with id {Id}", entity.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _context.CatalogItems.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                entity.IsActive = false;
                entity.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting catalog item with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.CatalogItems.AnyAsync(c => c.Id == id && c.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if catalog item exists with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<CatalogItem>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.CatalogItems
                .Include(c => c.CatalogBrand)
                .Include(c => c.CatalogType)
                .Where(c => c.IsActive && (c.Name.Contains(searchTerm) || (c.Description != null && c.Description.Contains(searchTerm))))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching catalog items with term {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<(IEnumerable<CatalogItem> Items, long TotalCount)> GetPaginatedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            var totalCount = await _context.CatalogItems.LongCountAsync(c => c.IsActive, cancellationToken);

            var items = await _context.CatalogItems
                .Include(c => c.CatalogBrand)
                .Include(c => c.CatalogType)
                .Where(c => c.IsActive)
                .OrderBy(c => c.Id)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated catalog items");
            throw;
        }
    }
}
