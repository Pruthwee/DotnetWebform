using eShopLegacyWebForms.Models;
using eShopLegacyWebForms.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShopLegacyWebForms.Catalog
{
    public class CreateModel : PageModel
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ICatalogService CatalogService { get; set; }

        [BindProperty]
        public CatalogItem CatalogItem { get; set; }

        public List<SelectListItem> Brands { get; set; }
        public List<SelectListItem> Types { get; set; }

        public CreateModel(ICatalogService catalogService)
        {
            CatalogService = catalogService;
        }

        public void OnGet()
        {
            _log.Info($"Now loading... /Catalog/Create.aspx");
            LoadDropdowns();
        }

        private void LoadDropdowns()
        {
            Brands = new List<SelectListItem>();
            foreach (var brand in CatalogService.GetCatalogBrands())
            {
                Brands.Add(new SelectListItem { Text = brand.Brand, Value = brand.Id.ToString() });
            }

            Types = new List<SelectListItem>();
            foreach (var type in CatalogService.GetCatalogTypes())
            {
                Types.Add(new SelectListItem { Text = type.Type, Value = type.Id.ToString() });
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var catalogItem = new CatalogItem
                {
                    Name = CatalogItem.Name,
                    Description = CatalogItem.Description,
                    CatalogBrandId = CatalogItem.CatalogBrandId,
                    CatalogTypeId = CatalogItem.CatalogTypeId,
                    Price = CatalogItem.Price,
                    AvailableStock = CatalogItem.AvailableStock,
                    RestockThreshold = CatalogItem.RestockThreshold,
                    MaxStockThreshold = CatalogItem.MaxStockThreshold
                };

                CatalogService.CreateCatalogItem(catalogItem);

                return Redirect("/");
            }

            LoadDropdowns();
            return Page();
        }
    }
}
