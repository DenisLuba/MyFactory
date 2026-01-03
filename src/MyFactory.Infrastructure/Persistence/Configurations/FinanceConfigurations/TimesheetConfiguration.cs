using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Infrastructure.Persistence.Configurations.FinanceConfigurations;

public class TimesheetConfiguration : IEntityTypeConfiguration<TimesheetEntity>
{
    public void Configure(EntityTypeBuilder<TimesheetEntity> builder)
    {
        builder.ToTable("TIMESHEETS");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.EmployeeId).IsRequired();
        builder.Property(t => t.DepartmentId).IsRequired();
        builder.Property(t => t.WorkDate).IsRequired();
        builder.Property(t => t.HoursWorked).IsRequired();
        builder.Property(t => t.Comment).HasMaxLength(500);

        builder.HasIndex(t => new { t.EmployeeId, t.WorkDate }).IsUnique();
    }
}
