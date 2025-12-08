using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Identity.Roles.Commands.UpdateRole;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Identity;
using Xunit;

namespace MyFactory.Application.Tests.Features.Identity.Roles;

public sealed class UpdateRoleCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateRole()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var role = new Role("Operator", "Initial");
        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();

        var handler = new UpdateRoleCommandHandler(context);
        var command = new UpdateRoleCommand(role.Id, "Supervisor", "Updated description");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Supervisor", result.Name);
        Assert.Equal("Updated description", result.Description);

        var stored = await context.Roles.SingleAsync();
        Assert.Equal("Supervisor", stored.Name);
        Assert.Equal("Updated description", stored.Description);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenNameAlreadyUsed()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var target = new Role("Operator");
        var other = new Role("Supervisor");
        await context.Roles.AddRangeAsync(target, other);
        await context.SaveChangesAsync();

        var handler = new UpdateRoleCommandHandler(context);
        var command = new UpdateRoleCommand(target.Id, "Supervisor", null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
