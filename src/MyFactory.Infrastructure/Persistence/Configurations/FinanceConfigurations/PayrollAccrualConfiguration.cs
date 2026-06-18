using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Infrastructure.Persistence.Configurations.FinanceConfigurations;

public class PayrollAccrualConfiguration : IEntityTypeConfiguration<PayrollAccrualEntity>
{
    public void Configure(EntityTypeBuilder<PayrollAccrualEntity> builder)
    {
        builder.ToTable("PAYROLL_ACCRUALS");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.EmployeeId).IsRequired();
        builder.Property(p => p.AccrualDate).IsRequired();
        builder.Property(p => p.HoursWorked).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(p => p.QtyPlanned).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(p => p.QtyProduced).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(p => p.QtyExtra).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(p => p.BaseAmount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(p => p.PremiumAmount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(p => p.TotalAmount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(p => p.AdjustmentReason).HasMaxLength(500);

        builder.HasIndex(p => new { p.EmployeeId, p.AccrualDate });
    }
}
