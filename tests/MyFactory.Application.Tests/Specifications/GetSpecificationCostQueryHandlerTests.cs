using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyFactory.Application.Features.Specifications;
using MyFactory.Application.Features.Specifications.Queries.GetSpecificationCost;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Operations;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Application.Tests.Specifications;

public class GetSpecificationCostQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCalculateCosts()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var specification = new Specification("SP-001", "Robe", 5m, SpecificationsStatusValues.Created, DateTime.UtcNow, null);

        var materialType = new MaterialType("Fabric");
        var supplier = new Supplier("Supplier", "contact");
        var material1 = new Material("Cotton", materialType.Id, "m");
        var material2 = new Material("Lace", materialType.Id, "m");
        var bom1 = specification.AddBomItem(material1.Id, 2m, "m", 5m);
        var bom2 = specification.AddBomItem(material2.Id, 1m, "m", null);

        var priceEntryPrimary = new MaterialPriceHistory(material1.Id, supplier.Id, 5m, new DateTime(2024, 1, 1), "DOC-Primary");
        var priceEntrySecondary = new MaterialPriceHistory(material2.Id, supplier.Id, 4m, new DateTime(2024, 1, 1), "DOC-1");
        var operation = new Operation("CUT", "Cutting", 5m, 10m, "Cut");
        var workshop = new Workshop("Main", "Sewing");
        var operationItem = specification.AddOperation(operation.Id, workshop.Id, 8m, 12m);
        var expense = new WorkshopExpenseHistory(workshop.Id, specification.Id, 15m, new DateTime(2024, 1, 1), null);

        await context.MaterialTypes.AddAsync(materialType);
        await context.Suppliers.AddAsync(supplier);
        await context.Materials.AddRangeAsync(material1, material2);
        await context.MaterialPriceHistoryEntries.AddRangeAsync(priceEntryPrimary, priceEntrySecondary);
        await context.Specifications.AddAsync(specification);
        await context.SpecificationBomItems.AddRangeAsync(bom1, bom2);
        await context.Operations.AddAsync(operation);
        await context.Workshops.AddAsync(workshop);
        await context.SpecificationOperations.AddAsync(operationItem);
        await context.WorkshopExpenseHistoryEntries.AddAsync(expense);
        await context.SaveChangesAsync();

        var handler = new GetSpecificationCostQueryHandler(context);
        var command = new GetSpecificationCostQuery(specification.Id, new DateTime(2024, 6, 1));

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(specification.Id, result.SpecificationId);
        Assert.Equal(2m * 5m + 1m * 4m, result.MaterialsCost);
        Assert.Equal(12m, result.OperationsCost);
        Assert.Equal(15m, result.WorkshopExpenses);
        Assert.Equal(result.MaterialsCost + result.OperationsCost + result.WorkshopExpenses, result.TotalCost);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSpecificationMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new GetSpecificationCostQueryHandler(context);
        var command = new GetSpecificationCostQuery(Guid.NewGuid(), DateTime.UtcNow);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
