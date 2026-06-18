using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Infrastructure.Persistence.Configurations.InventoryConfigurations;

public class FinishedGoodsScrapConfiguration : IEntityTypeConfiguration<FinishedGoodsScrapEntity>
{
    public void Configure(EntityTypeBuilder<FinishedGoodsScrapEntity> builder)
    {
        builder.ToTable("FINISHED_GOODS_SCRAP");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WarehouseId).IsRequired();
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.ProductionOrderId);
        builder.Property(x => x.Qty).IsRequired();
        builder.Property(x => x.ScrapDate).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.Reason).HasMaxLength(500);

        builder.HasOne(x => x.Warehouse)
            .WithMany(w => w.FinishedGoodsScraps)
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany(p => p.FinishedGoodsScraps)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ProductionOrder)
            .WithMany(po => po.FinishedGoodsScraps)
            .HasForeignKey(x => x.ProductionOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
