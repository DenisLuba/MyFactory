using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.FinishedGoods.Commands.CreateShipment;
using MyFactory.Application.Tests.Common;
using Xunit;

namespace MyFactory.Application.Tests.Features.FinishedGoods;

public class CreateShipmentCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Shipment_With_Items()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-SHIP-1");
        var customer = FinishedGoodsTestHelper.CreateCustomer("SHIP Customer");

        context.Specifications.Add(specification);
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        var handler = new CreateShipmentCommandHandler(context);

        var command = new CreateShipmentCommand(
            "SHIP-1001",
            customer.Id,
            new DateTime(2025, 5, 1),
            new List<CreateShipmentItemDto>
            {
                new(specification.Id, 3m, 75m)
            });

        var result = await handler.Handle(command, default);

        result.ShipmentNumber.Should().Be("SHIP-1001");
        result.Items.Should().HaveCount(1);
        result.TotalAmount.Should().Be(225m);

        var shipment = await context.Shipments.Include(s => s.Items).SingleAsync();
        shipment.Items.Should().HaveCount(1);
        shipment.Items.Should().ContainSingle(item => item.SpecificationId == specification.Id && item.Quantity == 3m);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Number_Not_Unique()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-SHIP-DUP");
        var customer = FinishedGoodsTestHelper.CreateCustomer("Ship Dup");
        var shipment = new MyFactory.Domain.Entities.Sales.Shipment("SHIP-2001", customer.Id, DateTime.UtcNow);
        shipment.AddItem(specification.Id, 1m, 10m);

        context.Specifications.Add(specification);
        context.Customers.Add(customer);
        context.Shipments.Add(shipment);
        await context.SaveChangesAsync();

        var handler = new CreateShipmentCommandHandler(context);

        var command = new CreateShipmentCommand(
            "SHIP-2001",
            customer.Id,
            DateTime.UtcNow,
            new List<CreateShipmentItemDto>
            {
                new(specification.Id, 2m, 15m)
            });

        var act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
