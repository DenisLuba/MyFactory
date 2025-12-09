using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(employee => employee.Id);

        builder.Property(employee => employee.FullName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(employee => employee.Position)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(employee => employee.Grade)
            .IsRequired();

        builder.Property(employee => employee.RatePerNormHour)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(employee => employee.PremiumPercent)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(employee => employee.IsActive)
            .HasDefaultValue(true);

        builder.Property(employee => employee.CreatedAt)
            .IsRequired();

        builder.Navigation(employee => employee.TimesheetEntries)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(employee => employee.PayrollEntries)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class TimesheetEntryConfiguration : IEntityTypeConfiguration<TimesheetEntry>
{
    public void Configure(EntityTypeBuilder<TimesheetEntry> builder)
    {
        builder.ToTable("TimesheetEntries");

        builder.HasKey(entry => entry.Id);

        builder.Property(entry => entry.WorkDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(entry => entry.HoursWorked)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(entry => entry.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(entry => entry.Employee)
            .WithMany(employee => employee.TimesheetEntries)
            .HasForeignKey(entry => entry.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entry => entry.ProductionOrder)
            .WithMany()
            .HasForeignKey(entry => entry.ProductionOrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PayrollEntryConfiguration : IEntityTypeConfiguration<PayrollEntry>
{
    public void Configure(EntityTypeBuilder<PayrollEntry> builder)
    {
        builder.ToTable("PayrollEntries");

        builder.HasKey(entry => entry.Id);

        builder.Property(entry => entry.PeriodStart)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(entry => entry.PeriodEnd)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(entry => entry.TotalHours)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(entry => entry.AccruedAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(entry => entry.PaidAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(entry => entry.Outstanding)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(entry => entry.Employee)
            .WithMany(employee => employee.PayrollEntries)
            .HasForeignKey(entry => entry.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
