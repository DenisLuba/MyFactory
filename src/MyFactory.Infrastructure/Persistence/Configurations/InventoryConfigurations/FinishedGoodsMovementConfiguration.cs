using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class FinishedGoodsMovementConfiguration : IEntityTypeConfiguration<FinishedGoodsMovementEntity>
{
    public void Configure(EntityTypeBuilder<FinishedGoodsMovementEntity> builder)
    {
        builder.ToTable("FINISHED_GOODS_MOVEMENTS");

        builder.HasKey(fgm => fgm.Id);

        builder.Property(fgm => fgm.FromWarehouseId).IsRequired();
        builder.Property(fgm => fgm.ToWarehouseId).IsRequired();
        builder.Property(fgm => fgm.MovementDate).IsRequired();
        builder.Property(fgm => fgm.CreatedBy).IsRequired();

        builder.HasMany(fgm => fgm.FinishedGoodsMovementItems)
            .WithOne(i => i.Movement)
            .HasForeignKey(i => i.MovementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(fgm => new { fgm.FromWarehouseId, fgm.ToWarehouseId, fgm.MovementDate });
    }
}
