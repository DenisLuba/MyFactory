using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Infrastructure.Persistence.Configurations.FinanceConfigurations;

public class CashAdvanceReturnConfiguration : IEntityTypeConfiguration<CashAdvanceReturnEntity>
{
    public void Configure(EntityTypeBuilder<CashAdvanceReturnEntity> builder)
    {
        builder.ToTable("CASH_ADVANCE_RETURNS");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CashAdvanceId).IsRequired();
        builder.Property(c => c.ReturnDate).IsRequired();
        builder.Property(c => c.Amount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(c => c.Description).IsRequired().HasMaxLength(500);

        builder.HasIndex(c => new { c.CashAdvanceId, c.ReturnDate });
    }
}
