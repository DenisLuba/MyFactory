using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.FinishedGoods.Queries.GetFinishedGoodsInventory;
using MyFactory.Application.Tests.Common;
using Xunit;

namespace MyFactory.Application.Tests.Features.FinishedGoods;

public class FinishedGoodsInventoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_All_Inventories()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specA = FinishedGoodsTestHelper.CreateSpecification("SPEC-INV-A");
        var specB = FinishedGoodsTestHelper.CreateSpecification("SPEC-INV-B");
        var warehouse = FinishedGoodsTestHelper.CreateWarehouse("FG-INV");
        context.Specifications.AddRange(specA, specB);
        context.Warehouses.Add(warehouse);
        context.FinishedGoodsInventories.Add(FinishedGoodsTestHelper.CreateInventory(specA, warehouse, 5m, 20m));
        context.FinishedGoodsInventories.Add(FinishedGoodsTestHelper.CreateInventory(specB, warehouse, 3m, 30m));
        await context.SaveChangesAsync();

        var handler = new GetFinishedGoodsInventoryQueryHandler(context);

        var result = await handler.Handle(new GetFinishedGoodsInventoryQuery(null, null), default);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_Should_Filter_By_Specification()
    {
        await using var context = TestApplicationDbContextFactory.Create();
        var specA = FinishedGoodsTestHelper.CreateSpecification("SPEC-INV-FILTER");
        var specB = FinishedGoodsTestHelper.CreateSpecification("SPEC-INV-OTHER");
        var warehouse = FinishedGoodsTestHelper.CreateWarehouse("FG-INV-FILTER");
        context.Specifications.AddRange(specA, specB);
        context.Warehouses.Add(warehouse);
        context.FinishedGoodsInventories.Add(FinishedGoodsTestHelper.CreateInventory(specA, warehouse, 5m, 20m));
        context.FinishedGoodsInventories.Add(FinishedGoodsTestHelper.CreateInventory(specB, warehouse, 3m, 30m));
        await context.SaveChangesAsync();

        var handler = new GetFinishedGoodsInventoryQueryHandler(context);

        var result = await handler.Handle(new GetFinishedGoodsInventoryQuery(specA.Id, null), default);

        result.Should().ContainSingle(dto => dto.SpecificationId == specA.Id);
    }
}
