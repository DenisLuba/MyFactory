using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Infrastructure.Persistence.Configurations.OrganizationConfigurations;

public class PositionConfiguration : IEntityTypeConfiguration<PositionEntity>
{
    public void Configure(EntityTypeBuilder<PositionEntity> builder)
    {
        builder.ToTable("POSITIONS");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Code)
            .HasMaxLength(50);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.DepartmentId).IsRequired();
        builder.Property(p => p.BaseNormPerHour).HasColumnType("numeric(18,2)");
        builder.Property(p => p.BaseRatePerNormHour).HasColumnType("numeric(18,2)");
        builder.Property(p => p.DefaultPremiumPercent).HasColumnType("numeric(18,2)");
        builder.Property(p => p.CanCut).IsRequired();
        builder.Property(p => p.CanSew).IsRequired();
        builder.Property(p => p.CanPackage).IsRequired();
        builder.Property(p => p.CanHandleMaterials).IsRequired();

        builder.HasIndex(p => p.Name).IsUnique();
        builder.HasIndex(p => p.Code).IsUnique();

        builder.HasMany(p => p.Employees)
            .WithOne(e => e.Position)
            .HasForeignKey(e => e.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
