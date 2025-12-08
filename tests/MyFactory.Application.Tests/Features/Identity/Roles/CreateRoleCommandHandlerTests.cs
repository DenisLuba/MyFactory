using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Identity.Roles.Commands.CreateRole;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Identity;
using Xunit;

namespace MyFactory.Application.Tests.Features.Identity.Roles;

public sealed class CreateRoleCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateRole()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new CreateRoleCommandHandler(context);
        var command = new CreateRoleCommand("Operator", "Manages shop floor");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Operator", result.Name);
        Assert.Equal("Manages shop floor", result.Description);
        Assert.NotEqual(Guid.Empty, result.Id);

        var stored = await context.Roles.SingleAsync();
        Assert.Equal(result.Id, stored.Id);
        Assert.Equal("Operator", stored.Name);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenNameExists()
    {
        using var context = TestApplicationDbContextFactory.Create();
        await context.Roles.AddAsync(new Role("Operator"));
        await context.SaveChangesAsync();

        var handler = new CreateRoleCommandHandler(context);
        var command = new CreateRoleCommand("Operator", null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
