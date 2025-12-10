using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.FinishedGoods.Commands.CreateCustomerReturn;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Sales;
using MyFactory.Domain.Enums;
using Xunit;

namespace MyFactory.Application.Tests.Features.FinishedGoods;

public class CreateCustomerReturnCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Return_With_Items()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-RET-1");
        var customer = FinishedGoodsTestHelper.CreateCustomer("Return Customer");

        context.Specifications.Add(specification);
        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        var handler = new CreateCustomerReturnCommandHandler(context);

        var command = new CreateCustomerReturnCommand(
            "RET-1001",
            customer.Id,
            new DateOnly(2025, 7, 1),
            "Damaged",
            new List<CreateCustomerReturnItemDto>
            {
                new(specification.Id, 2m, "Scrap")
            });

        var result = await handler.Handle(command, default);

        result.ReturnNumber.Should().Be("RET-1001");
        result.Items.Should().HaveCount(1);
        result.Status.Should().Be(ReturnStatus.Draft.ToString());

        var stored = await context.CustomerReturns.Include(r => r.Items).SingleAsync();
        stored.Items.Should().HaveCount(1);
        stored.Items.Should().ContainSingle(item => item.SpecificationId == specification.Id && item.Quantity == 2m);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Number_Not_Unique()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-RET-DUP");
        var customer = FinishedGoodsTestHelper.CreateCustomer("Return Dup");
        var existing = new MyFactory.Domain.Entities.Sales.CustomerReturn("RET-2001", customer.Id, DateOnly.FromDateTime(DateTime.UtcNow), "Reason");
        existing.AddItem(specification.Id, 1m, "Scrap");

        context.Specifications.Add(specification);
        context.Customers.Add(customer);
        context.CustomerReturns.Add(existing);
        await context.SaveChangesAsync();

        var handler = new CreateCustomerReturnCommandHandler(context);

        var command = new CreateCustomerReturnCommand(
            "RET-2001",
            customer.Id,
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Duplicate",
            new List<CreateCustomerReturnItemDto>
            {
                new(specification.Id, 1m, "Repair")
            });

        var act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
