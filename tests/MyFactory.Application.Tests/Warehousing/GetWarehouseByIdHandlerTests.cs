using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.Warehousing.Queries.GetWarehouseById;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Tests.Warehousing;

public sealed class GetWarehouseByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnWarehouse()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var warehouse = new Warehouse("Main", "Raw", "A1");
        await context.Warehouses.AddAsync(warehouse);
        await context.SaveChangesAsync();

        var handler = new GetWarehouseByIdQueryHandler(context);

        var result = await handler.Handle(new GetWarehouseByIdQuery(warehouse.Id), CancellationToken.None);

        result.Name.Should().Be("Main");
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenNotFound()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new GetWarehouseByIdQueryHandler(context);

        var act = async () => await handler.Handle(new GetWarehouseByIdQuery(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
