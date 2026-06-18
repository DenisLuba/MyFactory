using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Infrastructure.Persistence.Configurations.OrganizationConfigurations;

public class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPositionEntity>
{
    public void Configure(EntityTypeBuilder<DepartmentPositionEntity> builder)
    {
        builder.ToTable("DEPARTMENT_POSITIONS");

        builder.HasKey(dp => new { dp.DepartmentId, dp.PositionId });

        builder.HasOne(dp => dp.Department)
            .WithMany(d => d.DepartmentPositions)
            .HasForeignKey(dp => dp.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(dp => dp.Position)
            .WithMany(p => p.DepartmentPositions)
            .HasForeignKey(dp => dp.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
