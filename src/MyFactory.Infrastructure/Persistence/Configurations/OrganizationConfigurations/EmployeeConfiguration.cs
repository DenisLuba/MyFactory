using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Infrastructure.Persistence.Configurations.OrganizationConfigurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.ToTable("EMPLOYEES");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.PositionId).IsRequired();
        builder.Property(e => e.DepartmentId).IsRequired();
        builder.Property(e => e.Grade).IsRequired();
        builder.Property(e => e.RatePerNormHour).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(e => e.PremiumPercent).HasColumnType("numeric(18,2)");
        builder.Property(e => e.HiredAt).IsRequired();
        builder.Property(e => e.FiredAt);

        builder.HasOne(e => e.Department)
            .WithMany()
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.ContactLinks)
            .WithOne()
            .HasForeignKey(cl => cl.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.CuttingOperations)
            .WithOne(co => co.Employee)
            .HasForeignKey(co => co.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.PackagingOperations)
            .WithOne(po => po.Employee)
            .HasForeignKey(po => po.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.SewingOperations)
            .WithOne(so => so.Employee)
            .HasForeignKey(so => so.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Timesheets)
            .WithOne(t => t.Employee)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.PayrollAccruals)
            .WithOne(pa => pa.Employee)
            .HasForeignKey(pa => pa.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.PayrollPayments)
            .WithOne(pp => pp.Employee)
            .HasForeignKey(pp => pp.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.CashAdvances)
            .WithOne(ca => ca.Employee)
            .HasForeignKey(ca => ca.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
