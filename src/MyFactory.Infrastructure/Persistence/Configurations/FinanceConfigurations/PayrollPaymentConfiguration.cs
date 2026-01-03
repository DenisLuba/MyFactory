using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Infrastructure.Persistence.Configurations.FinanceConfigurations;

public class PayrollPaymentConfiguration : IEntityTypeConfiguration<PayrollPaymentEntity>
{
    public void Configure(EntityTypeBuilder<PayrollPaymentEntity> builder)
    {
        builder.ToTable("PAYROLL_PAYMENTS");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.EmployeeId).IsRequired();
        builder.Property(p => p.PaymentDate).IsRequired();
        builder.Property(p => p.Amount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();

        builder.HasIndex(p => new { p.EmployeeId, p.PaymentDate });
    }
}
