using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Shifts;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class ShiftPlanConfiguration : IEntityTypeConfiguration<ShiftPlan>
{
    public void Configure(EntityTypeBuilder<ShiftPlan> builder)
    {
        builder.ToTable("ShiftPlans");

        builder.HasKey(plan => plan.Id);

        builder.Property(plan => plan.ShiftType)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(plan => plan.ShiftDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(plan => plan.PlannedQuantity)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.HasOne(plan => plan.Employee)
            .WithMany()
            .HasForeignKey(plan => plan.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(plan => plan.Specification)
            .WithMany()
            .HasForeignKey(plan => plan.SpecificationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(plan => new { plan.EmployeeId, plan.ShiftDate })
            .IsUnique();
    }
}

public class ShiftResultConfiguration : IEntityTypeConfiguration<ShiftResult>
{
    public void Configure(EntityTypeBuilder<ShiftResult> builder)
    {
        builder.ToTable("ShiftResults");

        builder.HasKey(result => result.Id);

        builder.Property(result => result.ActualQuantity)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.Property(result => result.HoursWorked)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(result => result.RecordedAt)
            .IsRequired();

        builder.HasOne(result => result.ShiftPlan)
            .WithMany()
            .HasForeignKey(result => result.ShiftPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
