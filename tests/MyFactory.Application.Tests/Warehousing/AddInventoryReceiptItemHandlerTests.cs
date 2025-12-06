using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Warehousing.Commands.AddInventoryReceiptItem;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Tests.Warehousing;

public sealed class AddInventoryReceiptItemHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddItemToReceipt()
    {
        var setup = await SeedReceiptAsync();
        await using var context = setup.Context;
        var handler = new AddInventoryReceiptItemCommandHandler(context);

        var result = await handler.Handle(new AddInventoryReceiptItemCommand(setup.Receipt.Id, setup.Material.Id, 5m, 10m), CancellationToken.None);

        result.Items.Should().HaveCount(1);
        result.TotalAmount.Should().Be(50m);
        var stored = await context.InventoryReceipts
            .Include(r => r.Items)
            .AsNoTracking()
            .SingleAsync();
        stored.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenMaterialMissing()
    {
        var setup = await SeedReceiptAsync();
        await using var context = setup.Context;
        var handler = new AddInventoryReceiptItemCommandHandler(context);

        var act = async () => await handler.Handle(new AddInventoryReceiptItemCommand(setup.Receipt.Id, Guid.NewGuid(), 5m, 10m), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenQuantityInvalid()
    {
        var setup = await SeedReceiptAsync();
        await using var context = setup.Context;
        var handler = new AddInventoryReceiptItemCommandHandler(context);

        var act = async () => await handler.Handle(new AddInventoryReceiptItemCommand(setup.Receipt.Id, setup.Material.Id, 0m, 10m), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
    }

    private static async Task<(TestApplicationDbContext Context, InventoryReceipt Receipt, Material Material)> SeedReceiptAsync()
    {
        var context = TestApplicationDbContextFactory.Create();
        var supplier = new Supplier("Acme", "contact");
        var type = new MaterialType("Metals");
        var material = new Material("Steel", type.Id, "kg");
        var receipt = new InventoryReceipt("RC-001", supplier.Id, DateTime.UtcNow);

        await context.Suppliers.AddAsync(supplier);
        await context.MaterialTypes.AddAsync(type);
        await context.Materials.AddAsync(material);
        await context.InventoryReceipts.AddAsync(receipt);
        await context.SaveChangesAsync();

        return (context, receipt, material);
    }
}
