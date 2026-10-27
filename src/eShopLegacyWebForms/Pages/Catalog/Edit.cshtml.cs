using eShopLegacyWebForms.Models;
using eShopLegacyWebForms.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace eShopLegacyWebForms.Catalog
{
    public class EditModel : PageModel
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ICatalogService CatalogService { get; set; }

        [BindProperty]
        public CatalogItem Product { get; set; }

        public List<SelectListItem> Brands { get; set; }
        public List<SelectListItem> Types { get; set; }

        public EditModel(ICatalogService catalogService)
        {
            CatalogService = catalogService;
        }

        public void OnGet(int id)
        {
            _log.Info($"Now loading... /Catalog/Edit.aspx?id={id}");
            Product = CatalogService.FindCatalogItem(id);

            LoadDropdowns();
        }

        private void LoadDropdowns()
        {
            Brands = new List<SelectListItem>();
            foreach (var brand in CatalogService.GetCatalogBrands())
            {
                Brands.Add(new SelectListItem { Text = brand.Brand, Value = brand.Id.ToString(), Selected = brand.Id == Product.CatalogBrandId });
            }

            Types = new List<SelectListItem>();
            foreach (var type in CatalogService.GetCatalogTypes())
            {
                Types.Add(new SelectListItem { Text = type.Type, Value = type.Id.ToString(), Selected = type.Id == Product.CatalogTypeId });
            }
        }

        public IActionResult OnPost(int id)
        {
            if (ModelState.IsValid)
            {
                var catalogItem = new CatalogItem
                {
                    Id = id,
                    Name = Product.Name,
                    Description = Product.Description,
                    CatalogBrandId = Product.CatalogBrandId,
                    CatalogTypeId = Product.CatalogTypeId,
                    Price = Product.Price,
                    PictureFileName = Product.PictureFileName,
                    AvailableStock = Product.AvailableStock,
                    RestockThreshold = Product.RestockThreshold,
                    MaxStockThreshold = Product.MaxStockThreshold
                };
                CatalogService.UpdateCatalogItem(catalogItem);

                return Redirect("/");
            }

            LoadDropdowns();
            return Page();
        }
    }
}
