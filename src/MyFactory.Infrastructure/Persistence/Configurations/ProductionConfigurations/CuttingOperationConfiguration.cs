using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Infrastructure.Persistence.Configurations.ProductionConfigurations;

public class CuttingOperationConfiguration : IEntityTypeConfiguration<CuttingOperationEntity>
{
    public void Configure(EntityTypeBuilder<CuttingOperationEntity> builder)
    {
        builder.ToTable("CUTTING_OPERATIONS");

        builder.HasKey(co => co.Id);

        builder.Property(co => co.ProductionOrderId).IsRequired();
        builder.Property(co => co.EmployeeId).IsRequired();
        builder.Property(co => co.QtyPlanned).IsRequired();
        builder.Property(co => co.QtyCut).IsRequired();
        builder.Property(co => co.OperationDate).IsRequired();
    }
}
