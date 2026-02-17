using eShopLegacy.Domain.Entities;
using eShopLegacy.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShopLegacy.Web.Pages.Catalog;

public class IndexModel : PageModel
{
    private readonly ICatalogItemService _catalogService;
    private readonly ILogger<IndexModel> _logger;

    public const int DefaultPageIndex = 0;
    public const int DefaultPageSize = 10;

    public IndexModel(ICatalogItemService catalogService, ILogger<IndexModel> logger)
    {
        _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<CatalogItem> Items { get; set; } = Enumerable.Empty<CatalogItem>();
    public long TotalItems { get; set; }
    public int ActualPage { get; set; }
    public int ItemsPerPage { get; set; }
    public int TotalPages { get; set; }

    public async Task<IActionResult> OnGetAsync(int pageIndex = DefaultPageIndex, int pageSize = DefaultPageSize)
    {
        try
        {
            _logger.LogInformation("Loading catalog items page {PageIndex} with size {PageSize}", pageIndex, pageSize);

            var (items, totalCount) = await _catalogService.GetPaginatedAsync(pageIndex, pageSize);

            Items = items;
            TotalItems = totalCount;
            ActualPage = pageIndex;
            ItemsPerPage = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading catalog items");
            return RedirectToPage("/Error");
        }
    }
}
