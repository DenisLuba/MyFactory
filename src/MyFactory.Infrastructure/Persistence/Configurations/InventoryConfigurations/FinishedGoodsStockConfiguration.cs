using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class FinishedGoodsStockConfiguration : IEntityTypeConfiguration<FinishedGoodsStockEntity>
{
    public void Configure(EntityTypeBuilder<FinishedGoodsStockEntity> builder)
    {
        builder.ToTable("FINISHED_GOODS_STOCK");

        builder.HasKey(fgs => new { fgs.WarehouseId, fgs.ProductId });

        builder.Property(fgs => fgs.Qty).IsRequired();
    }
}
