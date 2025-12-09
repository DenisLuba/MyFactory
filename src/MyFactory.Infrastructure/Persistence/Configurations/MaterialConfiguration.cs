using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materials");

        builder.HasKey(material => material.Id);

        builder.Property(material => material.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(material => material.Unit)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(material => material.IsActive)
            .HasDefaultValue(true);

        builder.Property(material => material.CreatedAt)
            .IsRequired();

        builder.Property(material => material.UpdatedAt);

        builder.Property(material => material.IsDeleted)
            .HasDefaultValue(false);

        builder.HasOne(material => material.MaterialType)
            .WithMany(type => type.Materials)
            .HasForeignKey(material => material.MaterialTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(material => material.PriceHistory)
            .WithOne(history => history.Material)
            .HasForeignKey(history => history.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(material => material.PriceHistory)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasQueryFilter(material => !material.IsDeleted);

        builder.HasIndex(material => material.Name)
            .IsUnique();
    }
}

public class MaterialPriceHistoryConfiguration : IEntityTypeConfiguration<MaterialPriceHistory>
{
    public void Configure(EntityTypeBuilder<MaterialPriceHistory> builder)
    {
        builder.ToTable("MaterialPriceHistory");

        builder.HasKey(history => history.Id);

        builder.Property(history => history.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(history => history.DocRef)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(history => history.EffectiveFrom)
            .IsRequired();

        builder.Property(history => history.EffectiveTo);

        builder.Property(history => history.CreatedAt)
            .IsRequired();

        builder.HasOne(history => history.Material)
            .WithMany(material => material.PriceHistory)
            .HasForeignKey(history => history.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(history => history.Supplier)
            .WithMany(supplier => supplier.PriceEntries)
            .HasForeignKey(history => history.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
