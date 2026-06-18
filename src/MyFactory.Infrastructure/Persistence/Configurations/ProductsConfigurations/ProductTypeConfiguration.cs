using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Infrastructure.Persistence.Configurations.ProductsConfigurations;

public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductTypeEntity>
{
    public void Configure(EntityTypeBuilder<ProductTypeEntity> builder)
    {
        builder.ToTable("PRODUCT_TYPES");

        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Type)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pt => pt.Description).HasMaxLength(1000);      

        builder.HasIndex(pt => pt.Type).IsUnique();

        builder.HasMany(pt => pt.Products)
            .WithOne(p => p.ProductType)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
