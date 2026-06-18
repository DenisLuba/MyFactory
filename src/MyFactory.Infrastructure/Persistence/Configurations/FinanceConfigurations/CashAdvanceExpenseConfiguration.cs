using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Infrastructure.Persistence.Configurations.FinanceConfigurations;

public class CashAdvanceExpenseConfiguration : IEntityTypeConfiguration<CashAdvanceExpenseEntity>
{
    public void Configure(EntityTypeBuilder<CashAdvanceExpenseEntity> builder)
    {
        builder.ToTable("CASH_ADVANCE_EXPENSES");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CashAdvanceId).IsRequired();
        builder.Property(c => c.ExpenseDate).IsRequired();
        builder.Property(c => c.Amount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(c => c.Description).IsRequired().HasMaxLength(500);

        builder.HasIndex(c => new { c.CashAdvanceId, c.ExpenseDate });
    }
}
