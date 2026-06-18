using MyFactory.WebApi.Contracts.Shipments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Shipments;

public sealed class ShipmentListResponseExample : IExamplesProvider<IReadOnlyList<ShipmentListItemResponse>>
{
    public IReadOnlyList<ShipmentListItemResponse> GetExamples() => new List<ShipmentListItemResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            SalesOrderId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            CustomerId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            ShipmentDate: new DateTime(2024, 5, 1),
            Status: ShipmentStatusResponse.Confirmed,
            ItemsCount: 2,
            TotalQty: 15),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            SalesOrderId: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            CustomerId: Guid.Parse("44444444-4444-4444-4444-444444444444"),
            ShipmentDate: new DateTime(2024, 5, 3),
            Status: ShipmentStatusResponse.Shipped,
            ItemsCount: 1,
            TotalQty: 10)
    };
}
