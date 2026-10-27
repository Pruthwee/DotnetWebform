using eShopLegacyWebForms.Models;
using eShopLegacyWebForms.Services;
using eShopLegacyWebForms.ViewModel;
using log4net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;

namespace eShopLegacyWebForms.Pages
{
    public class IndexModel : PageModel
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ICatalogService _catalogService;

        public const int DefaultPageIndex = 0;
        public const int DefaultPageSize = 10;

        public PaginatedItemsViewModel<CatalogItem> Paginated { get; set; }

        public CatalogItem[] Items => Paginated?.Data?.ToArray() ?? new CatalogItem[0];

        public IndexModel(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public void OnGet(int index = DefaultPageIndex, int size = DefaultPageSize)
        {
            if (PaginationParamsAreSet(index, size))
            {
                Paginated = _catalogService.GetCatalogItemsPaginated(size, index);
                _log.Info($"Now loading... /Default.aspx?size={size}&index={index}");
            }
            else
            {
                Paginated = _catalogService.GetCatalogItemsPaginated(DefaultPageSize, DefaultPageIndex);
                _log.Info($"Now loading... /Default.aspx?size={DefaultPageSize}&index={DefaultPageIndex}");
            }
        }

        private bool PaginationParamsAreSet(int index, int size)
        {
            return index > 0 || size != DefaultPageSize;
        }
    }
}
