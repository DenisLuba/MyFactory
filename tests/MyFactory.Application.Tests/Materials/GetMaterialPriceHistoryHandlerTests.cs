using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyFactory.Application.Features.Materials.Queries.GetMaterialPriceHistory;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Tests.Materials;

public class GetMaterialPriceHistoryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnOrderedHistory()
    {
        await using var context = await SeedHistoryAsync();
        var materialId = await context.Materials.Select(material => material.Id).SingleAsync();

        var handler = new GetMaterialPriceHistoryQueryHandler(context);
        var result = await handler.Handle(new GetMaterialPriceHistoryQuery(materialId, null), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.True(result.First().EffectiveFrom > result.Last().EffectiveFrom);
    }

    [Fact]
    public async Task Handle_ShouldFilterBySupplier()
    {
        await using var context = await SeedHistoryAsync();
        var materialId = await context.Materials.Select(material => material.Id).SingleAsync();
        var supplierId = await context.Suppliers.OrderBy(s => s.Name).Select(s => s.Id).FirstAsync();

        var handler = new GetMaterialPriceHistoryQueryHandler(context);
        var result = await handler.Handle(new GetMaterialPriceHistoryQuery(materialId, supplierId), CancellationToken.None);

        Assert.All(result, dto => Assert.Equal(supplierId, dto.Supplier.Id));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenMaterialMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new GetMaterialPriceHistoryQueryHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new GetMaterialPriceHistoryQuery(Guid.NewGuid(), null), CancellationToken.None));
    }

    private static async Task<TestApplicationDbContext> SeedHistoryAsync()
    {
        var context = TestApplicationDbContextFactory.Create();
        var type = new MaterialType("Metals");
        var material = new Material("Steel", type.Id, "kg");
        var supplierA = new Supplier("Supplier A", "a@example.com");
        var supplierB = new Supplier("Supplier B", "b@example.com");
        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(material);
        await context.Suppliers.AddRangeAsync(supplierA, supplierB);
        await context.SaveChangesAsync();

        var entry1 = new MaterialPriceHistory(material.Id, supplierA.Id, 100m, new DateTime(2024, 1, 1), "DOC-1");
        entry1.SetEffectiveTo(new DateTime(2024, 1, 31));
        var entry2 = new MaterialPriceHistory(material.Id, supplierB.Id, 120m, new DateTime(2024, 2, 1), "DOC-2");
        await context.MaterialPriceHistoryEntries.AddRangeAsync(entry1, entry2);
        await context.SaveChangesAsync();

        return context;
    }
}
