using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.FinishedGoods.Commands.ConfirmShipmentPayment;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Sales;
using Xunit;

namespace MyFactory.Application.Tests.Features.FinishedGoods;

public class ConfirmShipmentPaymentCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Mark_Shipment_As_Paid()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-SHIP-PAY");
        var customer = FinishedGoodsTestHelper.CreateCustomer("Pay Customer");
        var shipment = FinishedGoodsTestHelper.CreateShippedOrder(
            customer,
            new DateTime(2025, 6, 1),
            (specification.Id, 5m, 100m));

        context.Specifications.Add(specification);
        context.Customers.Add(customer);
        context.Shipments.Add(shipment);
        await context.SaveChangesAsync();

        var handler = new ConfirmShipmentPaymentCommandHandler(context);

        var result = await handler.Handle(new ConfirmShipmentPaymentCommand(shipment.Id), default);

        result.Status.Should().Be(ShipmentStatus.Paid);

        var updated = await context.Shipments.SingleAsync();
        updated.Status.Should().Be(ShipmentStatus.Paid);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Not_Shipped()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-SHIP-NOT-SHIPPED");
        var customer = FinishedGoodsTestHelper.CreateCustomer("Unshipped");
        var shipment = new Shipment("SHIP-NOT-SHIPPED", customer.Id, DateTime.UtcNow);
        shipment.AddItem(specification.Id, 1m, 50m);

        context.Specifications.Add(specification);
        context.Customers.Add(customer);
        context.Shipments.Add(shipment);
        await context.SaveChangesAsync();

        var handler = new ConfirmShipmentPaymentCommandHandler(context);

        var act = async () => await handler.Handle(new ConfirmShipmentPaymentCommand(shipment.Id), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
