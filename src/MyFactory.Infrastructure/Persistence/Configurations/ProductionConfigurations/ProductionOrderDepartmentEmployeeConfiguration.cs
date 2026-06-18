using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Production;
namespace MyFactory.Infrastructure.Persistence.Configurations.ProductionConfigurations;

public class ProductionOrderDepartmentEmployeeConfiguration : IEntityTypeConfiguration<ProductionOrderDepartmentEmployeeEntity>
{
    public void Configure(EntityTypeBuilder<ProductionOrderDepartmentEmployeeEntity> builder)
    {
        builder.ToTable("PRODUCTION_ORDER_DEPARTMENT_EMPLOYEES");

        builder.HasKey(i => i.Id);
        builder.HasIndex(pode => new { pode.ProductionOrderId, pode.Stage, pode.EmployeeId, pode.WorkDate });

        builder.Property(pode => pode.ProductionOrderId).IsRequired();
        builder.Property(pode => pode.DepartmentId).IsRequired();
        builder.Property(pode => pode.EmployeeId).IsRequired();
        builder.Property(pode => pode.Stage).IsRequired();
        builder.Property(pode => pode.AssignedQty).IsRequired().HasPrecision(18, 2);
        builder.Property(pode => pode.CompletedQty).IsRequired().HasPrecision(18, 2);
        builder.Property(pode => pode.WorkDate).IsRequired();

        builder.HasOne(pode => pode.ProductionOrder)
            .WithMany(po => po.ProductionOrderDepartmentEmployees)
            .HasForeignKey(pode => pode.ProductionOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pode => pode.Department)
            .WithMany(d => d.ProductionOrderDepartmentEmployees)
            .HasForeignKey(pode => pode.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pode => pode.Employee)
            .WithMany(e => e.ProductionOrderDepartmentEmployees)
            .HasForeignKey(pode => pode.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
