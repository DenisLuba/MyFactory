using System;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Infrastructure.Repositories.Specifications;

public sealed class ProductionOrderWithDetailsSpecification : Specification<ProductionOrder>
{
    public ProductionOrderWithDetailsSpecification(Guid orderId)
        : base(order => order.Id == orderId)
    {
        AddInclude(order => order.Allocations);
        AddInclude(order => order.Stages);
        AddInclude($"{nameof(ProductionOrder.Stages)}.{nameof(ProductionStage.Assignments)}");
        AsNoTrackingQuery();
    }
}
