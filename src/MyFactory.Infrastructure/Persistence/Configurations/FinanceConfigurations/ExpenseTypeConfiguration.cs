using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Infrastructure.Persistence.Configurations.FinanceConfigurations;

public class ExpenseTypeConfiguration : IEntityTypeConfiguration<ExpenseTypeEntity>
{
    public void Configure(EntityTypeBuilder<ExpenseTypeEntity> builder)
    {
        builder.ToTable("EXPENSE_TYPES");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(500);

        builder.HasIndex(e => e.Name).IsUnique();
    }
}
