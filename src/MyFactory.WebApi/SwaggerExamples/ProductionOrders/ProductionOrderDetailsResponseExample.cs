using MyFactory.Domain.Entities.Production;
using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ProductionOrderDetailsResponseExample : IExamplesProvider<ProductionOrderDetailsResponse>
{
    public ProductionOrderDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        ProductionOrderNumber: "PO-001",
        SalesOrderItemId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
        DepartmentId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
        QtyPlanned: 120,
        QtyCut: 100,
        QtySewn: 80,
        QtyPacked: 70,
        QtyFinished: 70,
        Status: ProductionOrderStatus.Sewing);
}
