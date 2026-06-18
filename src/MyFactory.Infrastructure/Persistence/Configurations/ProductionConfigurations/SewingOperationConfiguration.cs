using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Infrastructure.Persistence.Configurations.ProductionConfigurations;

public class SewingOperationConfiguration : IEntityTypeConfiguration<SewingOperationEntity>
{
    public void Configure(EntityTypeBuilder<SewingOperationEntity> builder)
    {
        builder.ToTable("SEWING_OPERATIONS");

        builder.HasKey(so => so.Id);
        builder.HasIndex(so => so.AssignmentId);

        builder.Property(so => so.ProductionOrderId).IsRequired();
        builder.Property(so => so.EmployeeId).IsRequired();
        builder.Property(so => so.AssignmentId).IsRequired();
        builder.Property(so => so.QtyPlanned).IsRequired().HasPrecision(18, 2);
        builder.Property(so => so.QtySewn).IsRequired().HasPrecision(18, 2);
        builder.Property(so => so.HoursWorked).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(so => so.OperationDate).IsRequired();

        builder.HasOne(so => so.Assignment)
            .WithMany(a => a.SewingOperations)
            .HasForeignKey(so => so.AssignmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
