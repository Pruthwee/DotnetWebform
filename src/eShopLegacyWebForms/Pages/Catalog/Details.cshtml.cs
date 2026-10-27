using eShopLegacyWebForms.Models;
using eShopLegacyWebForms.Services;
using log4net;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShopLegacyWebForms.Catalog
{
    public class DetailsModel : PageModel
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ICatalogService CatalogService { get; set; }

        public CatalogItem Product { get; set; }

        public DetailsModel(ICatalogService catalogService)
        {
            CatalogService = catalogService;
        }

        public void OnGet(int id)
        {
            _log.Info($"Now loading... /Catalog/Details.aspx?id={id}");
            Product = CatalogService.FindCatalogItem(id);
        }
    }
}
