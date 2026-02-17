using eShopLegacy.Domain.Entities;
using eShopLegacy.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShopLegacy.Web.Pages.Catalog;

public class DeleteModel : PageModel
{
    private readonly ICatalogItemService _catalogService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ICatalogItemService catalogService, ILogger<DeleteModel> logger)
    {
        _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public CatalogItem? CatalogItem { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            _logger.LogInformation("Loading catalog item for delete with id {Id}", id);

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
            _logger.LogError(ex, "Error loading catalog item for delete with id {Id}", id);
            return RedirectToPage("/Error");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (CatalogItem == null || CatalogItem.Id == 0)
        {
            return NotFound();
        }

        try
        {
            _logger.LogInformation("Deleting catalog item with id {Id}", CatalogItem.Id);

            await _catalogService.DeleteAsync(CatalogItem.Id);

            _logger.LogInformation("Successfully deleted catalog item with id {Id}", CatalogItem.Id);

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting catalog item with id {Id}", CatalogItem?.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the catalog item.");
            return Page();
        }
    }
}
