using eShopLegacy.Domain.Entities;
using eShopLegacy.Domain.Interfaces.Repositories;
using eShopLegacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eShopLegacy.Infrastructure.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly CatalogDbContext _context;
    private readonly ILogger<CatalogBrandRepository> _logger;

    public CatalogBrandRepository(CatalogDbContext context, ILogger<CatalogBrandRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<CatalogBrand>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.CatalogBrands
                .Where(b => b.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all catalog brands");
            throw;
        }
    }

    public async Task<CatalogBrand?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.CatalogBrands
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving catalog brand with id {Id}", id);
            throw;
        }
    }

    public async Task<CatalogBrand> AddAsync(CatalogBrand entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.CatalogBrands.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding catalog brand");
            throw;
        }
    }

    public async Task UpdateAsync(CatalogBrand entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.CatalogBrands.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating catalog brand with id {Id}", entity.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _context.CatalogBrands.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                entity.IsActive = false;
                entity.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting catalog brand with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.CatalogBrands.AnyAsync(b => b.Id == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if catalog brand exists with id {Id}", id);
            throw;
        }
    }
}
