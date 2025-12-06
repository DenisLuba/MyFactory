using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.PurchaseRequests.Queries.GetPurchaseRequestById;
using MyFactory.Application.Features.PurchaseRequests.Queries.GetPurchaseRequests;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Tests.Features.PurchaseRequests;

public class PurchaseRequestQueryHandlerTests
{
    [Fact]
    public async Task GetPurchaseRequestByIdQueryHandler_Returns_Items_With_Material_Names()
    {
        var (context, draftRequest, _, material) = await CreateContextWithPurchaseRequestsAsync();
        await using var dbContext = context;
        var handler = new GetPurchaseRequestByIdQueryHandler(dbContext);

        var result = await handler.Handle(new GetPurchaseRequestByIdQuery(draftRequest.Id), default);

        result.Items.Should().ContainSingle()
            .Which.MaterialName.Should().Be(material.Name);
    }

    [Fact]
    public async Task GetPurchaseRequestsQueryHandler_Filters_By_Status()
    {
        var (context, _, approvedRequest, _) = await CreateContextWithPurchaseRequestsAsync();
        await using var dbContext = context;
        var handler = new GetPurchaseRequestsQueryHandler(dbContext);

        var result = await handler.Handle(new GetPurchaseRequestsQuery(PurchaseRequestStatus.Approved), default);

        result.Should().ContainSingle(pr => pr.Id == approvedRequest.Id);
    }

    private static async Task<(
        TestApplicationDbContext Context,
        PurchaseRequest DraftRequest,
        PurchaseRequest ApprovedRequest,
        Material DraftMaterial)> CreateContextWithPurchaseRequestsAsync()
    {
        var context = TestApplicationDbContextFactory.Create();
        var materialType = new MaterialType("Metals");
        var steel = new Material("Steel", materialType.Id, "kg");
        var copper = new Material("Copper", materialType.Id, "kg");

        context.MaterialTypes.Add(materialType);
        context.Materials.AddRange(steel, copper);

        var draftRequest = new PurchaseRequest("PR-DR", DateTime.UtcNow);
        draftRequest.AddItem(steel.Id, 3m);

        var approvedRequest = new PurchaseRequest("PR-AP", DateTime.UtcNow);
        approvedRequest.AddItem(copper.Id, 7m);
        approvedRequest.Submit();
        approvedRequest.Approve();

        context.PurchaseRequests.AddRange(draftRequest, approvedRequest);
        await context.SaveChangesAsync();

        return (context, draftRequest, approvedRequest, steel);
    }
}
