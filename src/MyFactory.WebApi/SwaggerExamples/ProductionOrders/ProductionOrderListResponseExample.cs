using MyFactory.Domain.Entities.Production;
using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ProductionOrderListResponseExample : IExamplesProvider<IReadOnlyList<ProductionOrderListItemResponse>>
{
    public IReadOnlyList<ProductionOrderListItemResponse> GetExamples() => new List<ProductionOrderListItemResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            ProductionOrderNumber: "PO-001",
            SalesOrderNumber: "SO-010",
            ProductName: "Пижама женская",
            QtyPlanned: 120,
            QtyFinished: 80,
            Status: ProductionOrderStatus.Sewing),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            ProductionOrderNumber: "PO-002",
            SalesOrderNumber: "SO-011",
            ProductName: "Халат махровый",
            QtyPlanned: 60,
            QtyFinished: 0,
            Status: ProductionOrderStatus.New)
    };
}
