using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Infrastructure.Persistence.Configurations.FinanceConfigurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<ExpenseEntity>
{
    public void Configure(EntityTypeBuilder<ExpenseEntity> builder)
    {
        builder.ToTable("EXPENSES");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ExpenseTypeId).IsRequired();
        builder.Property(e => e.ExpenseDate).IsRequired();
        builder.Property(e => e.Amount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.CreatedBy).IsRequired();

        builder.HasIndex(e => e.ExpenseDate);
    }
}
