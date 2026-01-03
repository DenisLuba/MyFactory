using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Infrastructure.Persistence.Configurations.ProductsConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("PRODUCTS");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Sku)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Version);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.PlanPerHour);
        builder.Property(p => p.PayrollRuleId);

        builder.HasIndex(p => p.Sku).IsUnique();

        builder.HasMany(p => p.ProductDepartmentCosts)
            .WithOne(pdc => pdc.Product)
            .HasForeignKey(pdc => pdc.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ProductMaterials)
            .WithOne(pm => pm.Product)
            .HasForeignKey(pm => pm.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.FinishedGoods)
            .WithOne(fg => fg.Product)
            .HasForeignKey(fg => fg.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.WarehouseProducts)
            .WithOne(wp => wp.Product)
            .HasForeignKey(wp => wp.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.FinishedGoodsMovementItems)
            .WithOne(fgmi => fgmi.Product)
            .HasForeignKey(fgmi => fgmi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ShipmentItems)
            .WithOne(si => si.Product)
            .HasForeignKey(si => si.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.FinishedGoodsStocks)
            .WithOne(fgs => fgs.Product)
            .HasForeignKey(fgs => fgs.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ShipmentReturnItems)
            .WithOne(sri => sri.Product)
            .HasForeignKey(sri => sri.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ProductImages)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
