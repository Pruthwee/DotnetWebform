using eShopLegacyWebForms.Models;
using eShopLegacyWebForms.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShopLegacyWebForms.Catalog
{
    public class DeleteModel : PageModel
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ICatalogService CatalogService { get; set; }

        public CatalogItem ProductToDelete { get; set; }

        public DeleteModel(ICatalogService catalogService)
        {
            CatalogService = catalogService;
        }

        public void OnGet(int id)
        {
            _log.Info($"Now loading... /Catalog/Delete.aspx?id={id}");
            ProductToDelete = CatalogService.FindCatalogItem(id);
        }

        public IActionResult OnPost(int id)
        {
            ProductToDelete = CatalogService.FindCatalogItem(id);
            CatalogService.RemoveCatalogItem(ProductToDelete);

            return Redirect("/");
        }
    }
}
