using System;
using MyFactory.Domain.Entities.FinishedGoods;

namespace MyFactory.Infrastructure.Repositories.Specifications;

public sealed class FinishedGoodsInventoryBySpecificationAndWarehouseSpecification : Specification<FinishedGoodsInventory>
{
    public FinishedGoodsInventoryBySpecificationAndWarehouseSpecification(Guid specificationId, Guid warehouseId)
        : base(inventory => inventory.SpecificationId == specificationId && inventory.WarehouseId == warehouseId)
    {
        AddInclude(inventory => inventory.Specification!);
        AddInclude(inventory => inventory.Warehouse!);
        AsNoTrackingQuery();
    }
}
