using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Reports;
using MyFactory.Infrastructure.Persistence.Constants;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class AdvanceConfiguration : IEntityTypeConfiguration<Advance>
{
    public void Configure(EntityTypeBuilder<Advance> builder)
    {
        builder.ToTable("Advances");

        builder.HasKey(advance => advance.Id);

        builder.Property(advance => advance.Amount)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(advance => advance.IssuedAt)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(advance => advance.Description)
            .HasMaxLength(Advance.DescriptionMaxLength);

        builder.Property(advance => advance.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(advance => advance.ClosedAt)
            .HasColumnType("date");

        builder.HasOne(advance => advance.Employee)
            .WithMany()
            .HasForeignKey(advance => advance.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(advance => advance.Reports)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class AdvanceReportConfiguration : IEntityTypeConfiguration<AdvanceReport>
{
    public void Configure(EntityTypeBuilder<AdvanceReport> builder)
    {
        builder.ToTable("AdvanceReports");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.Description)
            .IsRequired()
            .HasMaxLength(AdvanceReport.DescriptionMaxLength);

        builder.Property(report => report.Amount)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(report => report.ReportedAt)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(report => report.SpentAt)
            .HasColumnType("date")
            .IsRequired();

        builder.HasOne(report => report.Advance)
            .WithMany(advance => advance.Reports)
            .HasForeignKey(report => report.AdvanceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(report => report.File)
            .WithMany()
            .HasForeignKey(report => report.FileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ExpenseTypeConfiguration : IEntityTypeConfiguration<ExpenseType>
{
    public void Configure(EntityTypeBuilder<ExpenseType> builder)
    {
        builder.ToTable("ExpenseTypes");

        builder.HasKey(expenseType => expenseType.Id);

        builder.Property(expenseType => expenseType.Name)
            .IsRequired()
            .HasMaxLength(FieldLengths.ShortText);

        builder.Property(expenseType => expenseType.Category)
            .IsRequired()
            .HasMaxLength(FieldLengths.ShortText);

        builder.HasIndex(expenseType => expenseType.Name)
            .IsUnique();
    }
}

public class OverheadMonthlyConfiguration : IEntityTypeConfiguration<OverheadMonthly>
{
    public void Configure(EntityTypeBuilder<OverheadMonthly> builder)
    {
        builder.ToTable("OverheadMonthly");

        builder.HasKey(overhead => overhead.Id);

        builder.Property(overhead => overhead.PeriodMonth)
            .IsRequired();

        builder.Property(overhead => overhead.PeriodYear)
            .IsRequired();

        builder.Property(overhead => overhead.Amount)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(overhead => overhead.Notes)
            .HasMaxLength(FieldLengths.Notes);

        builder.HasOne(overhead => overhead.ExpenseType)
            .WithMany()
            .HasForeignKey(overhead => overhead.ExpenseTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(overhead => new { overhead.PeriodYear, overhead.PeriodMonth, overhead.ExpenseTypeId })
            .IsUnique();
    }
}

public class RevenueReportConfiguration : IEntityTypeConfiguration<RevenueReport>
{
    public void Configure(EntityTypeBuilder<RevenueReport> builder)
    {
        builder.ToTable("RevenueReports");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.PeriodMonth)
            .IsRequired();

        builder.Property(report => report.PeriodYear)
            .IsRequired();

        builder.Property(report => report.Quantity)
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.Property(report => report.UnitPrice)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(report => report.TotalRevenue)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(report => report.IsPaid)
            .HasDefaultValue(false);

        builder.Property(report => report.PaymentDate)
            .HasColumnType("date");

        builder.HasOne(report => report.Specification)
            .WithMany()
            .HasForeignKey(report => report.SpecificationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(report => report.MonthlyProfit)
            .WithMany(monthly => monthly.RevenueReports)
            .HasForeignKey(report => report.MonthlyProfitId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class ProductionCostFactConfiguration : IEntityTypeConfiguration<ProductionCostFact>
{
    public void Configure(EntityTypeBuilder<ProductionCostFact> builder)
    {
        builder.ToTable("ProductionCostFacts");

        builder.HasKey(fact => fact.Id);

        builder.Property(fact => fact.PeriodMonth)
            .IsRequired();

        builder.Property(fact => fact.PeriodYear)
            .IsRequired();

        builder.Property(fact => fact.QuantityProduced)
            .HasColumnType(ColumnTypes.Quantity)
            .IsRequired();

        builder.Property(fact => fact.MaterialCost)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(fact => fact.LaborCost)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(fact => fact.OverheadCost)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(fact => fact.TotalCost)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.HasOne(fact => fact.Specification)
            .WithMany()
            .HasForeignKey(fact => fact.SpecificationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class MonthlyProfitConfiguration : IEntityTypeConfiguration<MonthlyProfit>
{
    public void Configure(EntityTypeBuilder<MonthlyProfit> builder)
    {
        builder.ToTable("MonthlyProfits");

        builder.HasKey(profit => profit.Id);

        builder.Property(profit => profit.PeriodMonth)
            .IsRequired();

        builder.Property(profit => profit.PeriodYear)
            .IsRequired();

        builder.Property(profit => profit.Revenue)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(profit => profit.ProductionCost)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(profit => profit.Overhead)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Property(profit => profit.Profit)
            .HasColumnType(ColumnTypes.Monetary)
            .IsRequired();

        builder.Navigation(profit => profit.RevenueReports)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(profit => new { profit.PeriodYear, profit.PeriodMonth })
            .IsUnique();
    }
}
