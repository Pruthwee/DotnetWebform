namespace eShopLegacy.Domain.Entities;

public class CatalogItem
{
    public const string DefaultPictureName = "dummy.png";

    public CatalogItem()
    {
        PictureFileName = DefaultPictureName;
        CreatedDate = DateTime.UtcNow;
        IsActive = true;
        CreatedBy = "System";
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string PictureFileName { get; set; } = DefaultPictureName;
    public string? PictureUri { get; set; }
    public int CatalogTypeId { get; set; }
    public virtual CatalogType? CatalogType { get; set; }
    public int CatalogBrandId { get; set; }
    public virtual CatalogBrand? CatalogBrand { get; set; }
    public int AvailableStock { get; set; }
    public int RestockThreshold { get; set; }
    public int MaxStockThreshold { get; set; }
    public bool OnReorder { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
}
