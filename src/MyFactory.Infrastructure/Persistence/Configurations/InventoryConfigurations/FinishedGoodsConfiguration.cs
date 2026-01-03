using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class FinishedGoodsConfiguration : IEntityTypeConfiguration<FinishedGoodsEntity>
{
    public void Configure(EntityTypeBuilder<FinishedGoodsEntity> builder)
    {
        builder.ToTable("FINISHED_GOODS");

        builder.HasKey(fg => fg.Id);

        builder.Property(fg => fg.ProductId).IsRequired();
        builder.Property(fg => fg.WarehouseId).IsRequired();
        builder.Property(fg => fg.ProductionOrderId).IsRequired();
        builder.Property(fg => fg.Qty).IsRequired();

        builder.HasIndex(fg => new { fg.ProductId, fg.WarehouseId, fg.ProductionOrderId });
    }
}
