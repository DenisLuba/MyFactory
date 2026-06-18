using MyFactory.Domain.Entities.Orders;
using MyFactory.Domain.Entities.Production;
using MyFactory.WebApi.Contracts.Common;
using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ProductionOrderListResponseExample : IExamplesProvider<ListResponse<ProductionOrderListItemResponse>>
{
    public ListResponse<ProductionOrderListItemResponse> GetExamples() => new ListResponse<ProductionOrderListItemResponse>
    (
        Items:
        [
            new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            CustomerName: "Вася",
            ProductionOrderNumber: 1,
            SalesOrderNumber: 10,
            ProductName: "",
            QtyPlanned: 120,
            QtyFinished: 80,
            Status: ProductionOrderStatus.Sewing),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            CustomerName: "ООО Энерго",
            ProductionOrderNumber: 2,
            SalesOrderNumber: 11,
            ProductName: "",
            QtyPlanned: 60,
            QtyFinished: 0,
            Status: ProductionOrderStatus.New)
        ],
        TotalCount: 2,
        Take: 30,
        Skip: 0,
        HasMore: false
    );
}
