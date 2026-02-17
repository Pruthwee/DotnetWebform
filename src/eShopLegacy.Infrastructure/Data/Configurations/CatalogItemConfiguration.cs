using eShopLegacy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopLegacy.Infrastructure.Data.Configurations;

public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("CatalogItems");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.Price)
            .HasPrecision(18, 2);

        builder.Property(e => e.PictureFileName)
            .HasMaxLength(200);

        builder.Property(e => e.PictureUri)
            .HasMaxLength(500);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(e => e.CatalogType)
            .WithMany(t => t.CatalogItems)
            .HasForeignKey(e => e.CatalogTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.CatalogBrand)
            .WithMany(b => b.CatalogItems)
            .HasForeignKey(e => e.CatalogBrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
