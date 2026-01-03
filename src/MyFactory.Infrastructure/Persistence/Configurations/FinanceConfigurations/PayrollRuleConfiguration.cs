using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Infrastructure.Persistence.Configurations.FinanceConfigurations;

public class PayrollRuleConfiguration : IEntityTypeConfiguration<PayrollRuleEntity>
{
    public void Configure(EntityTypeBuilder<PayrollRuleEntity> builder)
    {
        builder.ToTable("PAYROLL_RULES");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.EffectiveFrom).IsRequired();
        builder.Property(p => p.PremiumPercent).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(p => p.Description).IsRequired().HasMaxLength(500);

        builder.HasIndex(p => p.EffectiveFrom).IsUnique();

        builder.HasMany(p => p.Products)
            .WithOne()
            .HasForeignKey(p => p.PayrollRuleId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
