using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.PurchaseRequests.Commands;
using MyFactory.Application.Features.PurchaseRequests.Handlers;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Tests.Features.PurchaseRequests;

public class PurchaseRequestCommandHandlerTests
{
    [Fact]
    public async Task UpdatePurchaseRequestItemCommandHandler_Updates_Item_For_Draft()
    {
        var (context, purchaseRequest, material, alternateMaterial) = await CreatePurchaseRequestAggregateAsync();
        await using var dbContext = context;
        var handler = new UpdatePurchaseRequestItemCommandHandler(dbContext);
        var targetItem = purchaseRequest.Items.First();

        var result = await handler.Handle(
            new UpdatePurchaseRequestItemCommand(purchaseRequest.Id, targetItem.Id, alternateMaterial.Id, 25m),
            default);

        result.Items.Should().ContainSingle()
            .Which.Quantity.Should().Be(25m);
        result.Items.Single().MaterialId.Should().Be(alternateMaterial.Id);
    }

    [Fact]
    public async Task UpdatePurchaseRequestItemCommandHandler_Throws_When_Status_Not_Draft()
    {
        var (context, purchaseRequest, material, _) = await CreatePurchaseRequestAggregateAsync();
        await using var dbContext = context;
        purchaseRequest.Submit();
        await dbContext.SaveChangesAsync();
        var handler = new UpdatePurchaseRequestItemCommandHandler(dbContext);
        var targetItem = purchaseRequest.Items.First();

        Func<Task> act = async () => await handler.Handle(
            new UpdatePurchaseRequestItemCommand(purchaseRequest.Id, targetItem.Id, material.Id, 10m),
            default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdatePurchaseRequestItemCommandHandler_Throws_When_Quantity_Not_Positive()
    {
        var (context, purchaseRequest, material, _) = await CreatePurchaseRequestAggregateAsync();
        await using var dbContext = context;
        var handler = new UpdatePurchaseRequestItemCommandHandler(dbContext);
        var targetItem = purchaseRequest.Items.First();

        Func<Task> act = async () => await handler.Handle(
            new UpdatePurchaseRequestItemCommand(purchaseRequest.Id, targetItem.Id, material.Id, -5m),
            default);

        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task RemovePurchaseRequestItemCommandHandler_Removes_Item_For_Draft()
    {
        var (context, purchaseRequest, _, _) = await CreatePurchaseRequestAggregateAsync();
        await using var dbContext = context;
        var handler = new RemovePurchaseRequestItemCommandHandler(dbContext);
        var targetItem = purchaseRequest.Items.First();

        var result = await handler.Handle(
            new RemovePurchaseRequestItemCommand(purchaseRequest.Id, targetItem.Id),
            default);

        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task RemovePurchaseRequestItemCommandHandler_Throws_When_Status_Not_Draft()
    {
        var (context, purchaseRequest, _, _) = await CreatePurchaseRequestAggregateAsync();
        await using var dbContext = context;
        purchaseRequest.Submit();
        await dbContext.SaveChangesAsync();
        var handler = new RemovePurchaseRequestItemCommandHandler(dbContext);
        var targetItem = purchaseRequest.Items.First();

        Func<Task> act = async () => await handler.Handle(
            new RemovePurchaseRequestItemCommand(purchaseRequest.Id, targetItem.Id),
            default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task SubmitPurchaseRequestCommandHandler_Throws_When_No_Items_Present()
    {
        var (context, purchaseRequest, _, _) = await CreatePurchaseRequestAggregateAsync();
        await using var dbContext = context;
        var removeHandler = new RemovePurchaseRequestItemCommandHandler(dbContext);
        var targetItem = purchaseRequest.Items.First();
        await removeHandler.Handle(new RemovePurchaseRequestItemCommand(purchaseRequest.Id, targetItem.Id), default);
        var submitHandler = new SubmitPurchaseRequestCommandHandler(dbContext);

        Func<Task> act = async () => await submitHandler.Handle(new SubmitPurchaseRequestCommand(purchaseRequest.Id), default);

        await act.Should().ThrowAsync<DomainException>();
    }

    [Theory]
    [InlineData(PurchaseRequestStatus.Draft)]
    [InlineData(PurchaseRequestStatus.Submitted)]
    public async Task CancelPurchaseRequestCommandHandler_Cancels_For_Allowed_Statuses(PurchaseRequestStatus status)
    {
        var (context, purchaseRequest, _, _) = await CreatePurchaseRequestAggregateAsync();
        await using var dbContext = context;

        if (status == PurchaseRequestStatus.Submitted)
        {
            purchaseRequest.Submit();
        }

        await dbContext.SaveChangesAsync();

        var handler = new CancelPurchaseRequestCommandHandler(dbContext);
        var result = await handler.Handle(new CancelPurchaseRequestCommand(purchaseRequest.Id), default);

        result.Status.Should().Be(PurchaseRequestStatus.Cancelled);
    }

    [Fact]
    public async Task CancelPurchaseRequestCommandHandler_Throws_When_Status_Not_Cancellable()
    {
        var (context, purchaseRequest, _, _) = await CreatePurchaseRequestAggregateAsync();
        await using var dbContext = context;
        purchaseRequest.Submit();
        purchaseRequest.Approve();
        await dbContext.SaveChangesAsync();

        var handler = new CancelPurchaseRequestCommandHandler(dbContext);

        Func<Task> act = async () => await handler.Handle(new CancelPurchaseRequestCommand(purchaseRequest.Id), default);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private static async Task<(
        TestApplicationDbContext Context,
        PurchaseRequest PurchaseRequest,
        Material PrimaryMaterial,
        Material SecondaryMaterial)> CreatePurchaseRequestAggregateAsync()
    {
        var context = TestApplicationDbContextFactory.Create();
        var materialType = new MaterialType("Metals");
        var steel = new Material("Steel", materialType.Id, "kg");
        var copper = new Material("Copper", materialType.Id, "kg");

        context.MaterialTypes.Add(materialType);
        context.Materials.AddRange(steel, copper);

        var purchaseRequest = new PurchaseRequest("PR-001", DateTime.UtcNow);
        purchaseRequest.AddItem(steel.Id, 5m);

        context.PurchaseRequests.Add(purchaseRequest);
        await context.SaveChangesAsync();

        return (context, purchaseRequest, steel, copper);
    }
}
