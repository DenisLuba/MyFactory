using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Warehousing.Commands.CreateWarehouse;
using MyFactory.Application.Tests.Common;

namespace MyFactory.Application.Tests.Warehousing;

public sealed class CreateWarehouseHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateWarehouse()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new CreateWarehouseCommandHandler(context);
        var command = new CreateWarehouseCommand("Main", "Raw", "A1");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Name.Should().Be("Main");
        var stored = await context.Warehouses.AsNoTracking().SingleAsync();
        stored.Type.Should().Be("Raw");
        stored.Location.Should().Be("A1");
    }

    [Fact]
    public void Validator_ShouldFail_WhenNameOrTypeMissing()
    {
        var validator = new CreateWarehouseCommandValidator();

        var result = validator.Validate(new CreateWarehouseCommand(string.Empty, string.Empty, ""));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}
