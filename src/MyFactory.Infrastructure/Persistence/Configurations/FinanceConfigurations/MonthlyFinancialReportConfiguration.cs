using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Infrastructure.Persistence.Configurations.FinanceConfigurations;

public class MonthlyFinancialReportConfiguration : IEntityTypeConfiguration<MonthlyFinancialReportEntity>
{
    public void Configure(EntityTypeBuilder<MonthlyFinancialReportEntity> builder)
    {
        builder.ToTable("MONTHLY_FINANCIAL_REPORTS");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.ReportYear).IsRequired();
        builder.Property(r => r.ReportMonth).IsRequired();
        builder.Property(r => r.TotalRevenue).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(r => r.PayrollExpenses).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(r => r.MaterialExpenses).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(r => r.OtherExpenses).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.CalculatedAt).IsRequired();
        builder.Property(r => r.CreatedBy).IsRequired();

        builder.HasIndex(r => new { r.ReportYear, r.ReportMonth }).IsUnique();
    }
}
