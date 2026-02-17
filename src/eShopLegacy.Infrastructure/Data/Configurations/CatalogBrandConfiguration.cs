using eShopLegacy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopLegacy.Infrastructure.Data.Configurations;

public class CatalogBrandConfiguration : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable("CatalogBrands");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Brand)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);
    }
}
