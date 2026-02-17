using eShopLegacy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShopLegacy.Infrastructure.Data.Configurations;

public class CatalogTypeConfiguration : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("CatalogTypes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Type)
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
