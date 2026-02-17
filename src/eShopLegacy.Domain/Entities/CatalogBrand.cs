namespace eShopLegacy.Domain.Entities;

public class CatalogBrand
{
    public CatalogBrand()
    {
        CreatedDate = DateTime.UtcNow;
        IsActive = true;
        CreatedBy = "System";
        CatalogItems = new HashSet<CatalogItem>();
    }

    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
    public virtual ICollection<CatalogItem> CatalogItems { get; set; }
}
