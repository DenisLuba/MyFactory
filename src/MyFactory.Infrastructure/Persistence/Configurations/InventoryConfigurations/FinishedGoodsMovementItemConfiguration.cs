using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class FinishedGoodsMovementItemConfiguration : IEntityTypeConfiguration<FinishedGoodsMovementItemEntity>
{
    public void Configure(EntityTypeBuilder<FinishedGoodsMovementItemEntity> builder)
    {
        builder.ToTable("FINISHED_GOODS_MOVEMENT_ITEMS");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.MovementId).IsRequired();
        builder.Property(i => i.ProductId).IsRequired();
        builder.Property(i => i.Qty).IsRequired();
    }
}
