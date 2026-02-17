using eShopLegacy.Domain.Entities;
using eShopLegacy.Domain.Interfaces.Repositories;
using eShopLegacy.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace eShopLegacy.Web.Pages.Catalog;

public class CreateModel : PageModel
{
    private readonly ICatalogItemService _catalogService;
    private readonly ICatalogBrandRepository _brandRepository;
    private readonly ICatalogTypeRepository _typeRepository;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        ICatalogItemService catalogService,
        ICatalogBrandRepository brandRepository,
        ICatalogTypeRepository typeRepository,
        ILogger<CreateModel> logger)
    {
        _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        _brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
        _typeRepository = typeRepository ?? throw new ArgumentNullException(nameof(typeRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public CatalogItemViewModel CatalogItem { get; set; } = new();

    public SelectList CatalogTypes { get; set; } = new SelectList(Enumerable.Empty<CatalogType>(), "Id", "Type");
    public SelectList CatalogBrands { get; set; } = new SelectList(Enumerable.Empty<CatalogBrand>(), "Id", "Brand");

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadSelectLists();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return Page();
        }

        try
        {
            var entity = new CatalogItem
            {
                Name = CatalogItem.Name,
                Description = CatalogItem.Description,
                Price = CatalogItem.Price,
                PictureFileName = CatalogItem.PictureFileName ?? CatalogItemViewModel.DefaultPictureName,
                CatalogTypeId = CatalogItem.CatalogTypeId,
                CatalogBrandId = CatalogItem.CatalogBrandId,
                AvailableStock = CatalogItem.AvailableStock,
                RestockThreshold = CatalogItem.RestockThreshold,
                MaxStockThreshold = CatalogItem.MaxStockThreshold,
                OnReorder = false
            };

            await _catalogService.CreateAsync(entity);

            _logger.LogInformation("Created catalog item: {Name}", entity.Name);

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating catalog item");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the catalog item.");
            await LoadSelectLists();
            return Page();
        }
    }

    private async Task LoadSelectLists()
    {
        var types = await _typeRepository.GetAllAsync();
        var brands = await _brandRepository.GetAllAsync();

        CatalogTypes = new SelectList(types, "Id", "Type");
        CatalogBrands = new SelectList(brands, "Id", "Brand");
    }

    public class CatalogItemViewModel
    {
        public const string DefaultPictureName = "dummy.png";

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0, 9999999999999999.99)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? PictureFileName { get; set; }

        [Required]
        [Display(Name = "Type")]
        public int CatalogTypeId { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public int CatalogBrandId { get; set; }

        [Required]
        [Range(0, 10000000)]
        [Display(Name = "Stock")]
        public int AvailableStock { get; set; }

        [Required]
        [Range(0, 10000000)]
        [Display(Name = "Restock")]
        public int RestockThreshold { get; set; }

        [Required]
        [Range(0, 10000000)]
        [Display(Name = "Max stock")]
        public int MaxStockThreshold { get; set; }
    }
}
