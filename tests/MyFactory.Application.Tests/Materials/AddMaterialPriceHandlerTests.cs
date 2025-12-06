using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MyFactory.Application.Features.Materials.Commands.AddMaterialPrice;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Tests.Materials;

public class AddMaterialPriceHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddPriceEntry()
    {
        await using var context = await BuildContextAsync();
        var material = await context.Materials.SingleAsync();
        var supplier = await context.Suppliers.SingleAsync();

        var handler = new AddMaterialPriceCommandHandler(context);
        var command = new AddMaterialPriceCommand(material.Id, supplier.Id, 125m, new DateTime(2024, 1, 1), new DateTime(2024, 6, 30), "DOC-001");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(125m, result.Price);
        Assert.Equal(supplier.Id, result.Supplier.Id);
        Assert.Equal(1, await context.MaterialPriceHistoryEntries.CountAsync());
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenMaterialMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        await context.Suppliers.AddAsync(new Supplier("Supplier", "contact"));
        await context.SaveChangesAsync();

        var handler = new AddMaterialPriceCommandHandler(context);
        var supplierId = await context.Suppliers.Select(s => s.Id).SingleAsync();
        var command = new AddMaterialPriceCommand(Guid.NewGuid(), supplierId, 50m, DateTime.UtcNow, null, "DOC");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSupplierMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var type = new MaterialType("Metals");
        await context.MaterialTypes.AddAsync(type);
        var material = new Material("Steel", type.Id, "kg");
        await context.Materials.AddAsync(material);
        await context.SaveChangesAsync();

        var handler = new AddMaterialPriceCommandHandler(context);
        var command = new AddMaterialPriceCommand(material.Id, Guid.NewGuid(), 50m, DateTime.UtcNow, null, "DOC");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenRangesOverlap()
    {
        await using var context = await BuildContextAsync();
        var material = await context.Materials.SingleAsync();
        var supplier = await context.Suppliers.SingleAsync();

        var existing = material.AddPrice(supplier.Id, 100m, new DateTime(2024, 1, 1), "DOC-EXISTING");
        existing.SetEffectiveTo(new DateTime(2024, 1, 31));
        await context.MaterialPriceHistoryEntries.AddAsync(existing);
        await context.SaveChangesAsync();

        var handler = new AddMaterialPriceCommandHandler(context);
        var command = new AddMaterialPriceCommand(material.Id, supplier.Id, 120m, new DateTime(2024, 1, 15), new DateTime(2024, 2, 15), "DOC-OVERLAP");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenEffectiveToBeforeEffectiveFrom()
    {
        await using var context = await BuildContextAsync();
        var material = await context.Materials.SingleAsync();
        var supplier = await context.Suppliers.SingleAsync();

        var handler = new AddMaterialPriceCommandHandler(context);
        var command = new AddMaterialPriceCommand(material.Id, supplier.Id, 120m, new DateTime(2024, 6, 1), new DateTime(2024, 5, 31), "DOC-INVALID");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    private static async Task<TestApplicationDbContext> BuildContextAsync()
    {
        var context = TestApplicationDbContextFactory.Create();
        var type = new MaterialType("Metals");
        var material = new Material("Steel", type.Id, "kg");
        var supplier = new Supplier("Supplier", "contact");
        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(material);
        await context.Suppliers.AddAsync(supplier);
        await context.SaveChangesAsync();
        return context;
    }
}
