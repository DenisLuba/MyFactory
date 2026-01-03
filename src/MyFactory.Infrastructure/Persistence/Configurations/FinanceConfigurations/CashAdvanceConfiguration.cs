using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Infrastructure.Persistence.Configurations.FinanceConfigurations;

public class CashAdvanceConfiguration : IEntityTypeConfiguration<CashAdvanceEntity>
{
    public void Configure(EntityTypeBuilder<CashAdvanceEntity> builder)
    {
        builder.ToTable("CASH_ADVANCES");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.EmployeeId).IsRequired();
        builder.Property(c => c.IssueDate).IsRequired();
        builder.Property(c => c.Amount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(c => c.Status).IsRequired();
        builder.Property(c => c.ClosedAt);

        builder.HasIndex(c => new { c.EmployeeId, c.IssueDate });
    }
}
