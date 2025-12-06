using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MyFactory.Application.Features.Warehousing.Queries.GetWarehouses;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Tests.Warehousing;

public sealed class GetWarehousesHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAllWarehouses()
    {
        using var context = TestApplicationDbContextFactory.Create();
        await context.Warehouses.AddRangeAsync(
            new Warehouse("Main", "Raw", "A1"),
            new Warehouse("Secondary", "Finished", "B1"));
        await context.SaveChangesAsync();

        var handler = new GetWarehousesQueryHandler(context);

        var result = await handler.Handle(new GetWarehousesQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().Contain(dto => dto.Name == "Main");
    }
}
