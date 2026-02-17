using eShopLegacy.Domain.Entities;
using eShopLegacy.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShopLegacy.Web.Pages.Catalog;

public class DetailsModel : PageModel
{
    private readonly ICatalogItemService _catalogService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ICatalogItemService catalogService, ILogger<DetailsModel> logger)
    {
        _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public CatalogItem? CatalogItem { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            _logger.LogInformation("Loading catalog item details for id {Id}", id);

            CatalogItem = await _catalogService.GetByIdAsync(id);

            if (CatalogItem == null)
            {
                _logger.LogWarning("Catalog item with id {Id} not found", id);
                return NotFound();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading catalog item details for id {Id}", id);
            return RedirectToPage("/Error");
        }
    }
}
