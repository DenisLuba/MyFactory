using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Infrastructure.Persistence.Configurations.ProductsConfigurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImageEntity>
{
    public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
    {
        builder.ToTable("PRODUCT_IMAGES");

        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.ProductId).IsRequired();
        builder.Property(pi => pi.FileName).IsRequired().HasMaxLength(255);
        builder.Property(pi => pi.Path).IsRequired().HasMaxLength(500);
        builder.Property(pi => pi.ContentType).HasMaxLength(200);
        builder.Property(pi => pi.SortOrder).IsRequired();
    }
}
