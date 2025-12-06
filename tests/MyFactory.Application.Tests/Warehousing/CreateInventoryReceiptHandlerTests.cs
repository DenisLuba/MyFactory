using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Warehousing.Commands.CreateInventoryReceipt;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Tests.Warehousing;

public sealed class CreateInventoryReceiptHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateReceiptWithoutItems()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var supplier = new Supplier("Acme", "contact");
        await context.Suppliers.AddAsync(supplier);
        await context.SaveChangesAsync();

        var handler = new CreateInventoryReceiptCommandHandler(context);
        var command = new CreateInventoryReceiptCommand("RC-001", supplier.Id, DateTime.UtcNow);

        var result = await handler.Handle(command, CancellationToken.None);

        result.ReceiptNumber.Should().Be("RC-001");
        result.Items.Should().BeEmpty();
        (await context.InventoryReceipts.AsNoTracking().CountAsync()).Should().Be(1);
    }
}
