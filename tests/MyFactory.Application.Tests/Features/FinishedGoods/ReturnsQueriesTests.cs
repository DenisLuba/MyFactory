using System;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.FinishedGoods.Queries.GetReturnById;
using MyFactory.Application.Features.FinishedGoods.Queries.GetReturns;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Sales;
using MyFactory.Domain.Enums;
using Xunit;

namespace MyFactory.Application.Tests.Features.FinishedGoods;

public class ReturnsQueriesTests
{
    [Fact]
    public async Task GetReturns_Should_Filter_By_Status()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-RET-QUERY");
        var customer = FinishedGoodsTestHelper.CreateCustomer("Return Query");
        var approved = new CustomerReturn("RET-APP", customer.Id, new DateOnly(2025, 10, 1), "Reason");
        approved.AddItem(specification.Id, 1m, "Scrap");
        approved.MarkAsReceived();

        var pending = new CustomerReturn("RET-PEN", customer.Id, new DateOnly(2025, 10, 2), "Reason");
        pending.AddItem(specification.Id, 1m, "Scrap");

        context.Specifications.Add(specification);
        context.Customers.Add(customer);
        context.CustomerReturns.AddRange(approved, pending);
        await context.SaveChangesAsync();

        var handler = new GetReturnsQueryHandler(context);

        var result = await handler.Handle(new GetReturnsQuery(ReturnStatus.Received.ToString()), default);

        result.Should().ContainSingle(dto => dto.Status == ReturnStatus.Received.ToString());
    }

    [Fact]
    public async Task GetReturnById_Should_Return_Return()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specification = FinishedGoodsTestHelper.CreateSpecification("SPEC-RET-CARD");
        var customer = FinishedGoodsTestHelper.CreateCustomer("Return Card");
        var customerReturn = new CustomerReturn("RET-CARD", customer.Id, new DateOnly(2025, 11, 1), "Reason");
        customerReturn.AddItem(specification.Id, 2m, "Repair");

        context.Specifications.Add(specification);
        context.Customers.Add(customer);
        context.CustomerReturns.Add(customerReturn);
        await context.SaveChangesAsync();

        var handler = new GetReturnByIdQueryHandler(context);

        var result = await handler.Handle(new GetReturnByIdQuery(customerReturn.Id), default);

        result.Items.Should().HaveCount(1);
        result.Customer.Should().NotBeNull();
    }
}
