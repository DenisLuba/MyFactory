using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Infrastructure.Persistence.Configurations.OrganizationConfigurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<DepartmentEntity>
{
    public void Configure(EntityTypeBuilder<DepartmentEntity> builder)
    {
        builder.ToTable("DEPARTMENTS");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Code)
            .HasMaxLength(50);

        builder.Property(d => d.Type).IsRequired();

        builder.HasIndex(d => d.Name).IsUnique();

        builder.HasMany(d => d.Positions)
            .WithOne(p => p.Department)
            .HasForeignKey(p => p.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.ProductDepartmentCosts)
            .WithOne(pdc => pdc.Department)
            .HasForeignKey(pdc => pdc.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.InventoryMovements)
            .WithOne(im => im.ToDepartment)
            .HasForeignKey(im => im.ToDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.ProductionOrders)
            .WithOne(po => po.Department)
            .HasForeignKey(po => po.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Timesheets)
            .WithOne(t => t.Department)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
