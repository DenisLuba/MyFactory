using System;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.FinishedGoods.Queries.GetShipmentById;
using MyFactory.Application.Features.FinishedGoods.Queries.GetShipments;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Sales;
using MyFactory.Domain.Enums;
using Xunit;

namespace MyFactory.Application.Tests.Features.FinishedGoods;

public class ShipmentsQueriesTests
{
    [Fact]
    public async Task GetShipments_Should_Filter_By_Status()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-SHIP-QUERY");
        var customer = FinishedGoodsTestHelper.CreateCustomer("Ship Query");
        var shipped = FinishedGoodsTestHelper.CreateShippedOrder(customer, new DateOnly(2025, 8, 1), (specification.Id, 2m, 30m));
        var draft = new Shipment("SHIP-DRAFT", customer.Id, new DateOnly(2025, 8, 2));
        draft.AddItem(specification.Id, 1m, 25m);

        context.Specifications.Add(specification);
        context.Customers.Add(customer);
        context.Shipments.AddRange(shipped, draft);
        await context.SaveChangesAsync();

        var handler = new GetShipmentsQueryHandler(context);

        var result = await handler.Handle(new GetShipmentsQuery(ShipmentStatus.Shipped.ToString()), default);

        result.Should().ContainSingle(dto => dto.Status == ShipmentStatus.Shipped.ToString());
    }

    [Fact]
    public async Task GetShipmentById_Should_Return_Shipment()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-SHIP-CARD");
        var customer = FinishedGoodsTestHelper.CreateCustomer("Ship Card");
        var shipment = FinishedGoodsTestHelper.CreateShippedOrder(customer, new DateOnly(2025, 9, 1), (specification.Id, 4m, 40m));

        context.Specifications.Add(specification);
        context.Customers.Add(customer);
        context.Shipments.Add(shipment);
        await context.SaveChangesAsync();

        var handler = new GetShipmentByIdQueryHandler(context);

        var result = await handler.Handle(new GetShipmentByIdQuery(shipment.Id), default);

        result.ShipmentNumber.Should().NotBeNullOrWhiteSpace();
        result.Items.Should().HaveCount(1);
    }
}
