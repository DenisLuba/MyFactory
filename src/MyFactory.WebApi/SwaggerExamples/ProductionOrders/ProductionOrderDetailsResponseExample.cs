using MyFactory.Domain.Entities.Production;
using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ProductionOrderDetailsResponseExample : IExamplesProvider<ProductionOrderDetailsResponse>
{
    public ProductionOrderDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        ProductionOrderNumber: 1,
        SalesOrderId: Guid.Parse("99999999-9999-9999-9999-999999999999"),
        SalesOrderItemId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
        ProductId: Guid.Parse("33333333-3333-3333-3333-333333333333"), 
        ProductName: "Платье",
        DepartmentId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
        DepartmentName: "Швейный цех",
        QtyPlanned: 120,
        QtyCut: 100,
        QtySewn: 80,
        QtyPacked: 70,
        QtyFinished: 70,
        Status: ProductionOrderStatus.Sewing);
}
