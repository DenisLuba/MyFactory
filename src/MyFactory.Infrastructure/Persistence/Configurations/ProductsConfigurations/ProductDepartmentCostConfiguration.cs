using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Infrastructure.Persistence.Configurations.ProductsConfigurations;

public class ProductDepartmentCostConfiguration : IEntityTypeConfiguration<ProductDepartmentCostEntity>
{
    public void Configure(EntityTypeBuilder<ProductDepartmentCostEntity> builder)
    {
        builder.ToTable("PRODUCT_DEPARTMENT_COSTS");

        builder.HasKey(pdc => pdc.Id);

        builder.Property(pdc => pdc.ProductId).IsRequired();
        builder.Property(pdc => pdc.DepartmentId).IsRequired();
        builder.Property(pdc => pdc.ExpensesPerUnit).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(pdc => pdc.CutCostPerUnit).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(pdc => pdc.SewingCostPerUnit).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(pdc => pdc.PackCostPerUnit).IsRequired().HasColumnType("numeric(18,2)");

        builder.HasIndex(pdc => new { pdc.ProductId, pdc.DepartmentId }).IsUnique();
    }
}
