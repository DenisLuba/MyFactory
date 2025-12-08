using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Identity.Roles.Commands.DeleteRole;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Identity;
using Xunit;

namespace MyFactory.Application.Tests.Features.Identity.Roles;

public sealed class DeleteRoleCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteRole_WhenNoUsers()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var role = new Role("Operator");
        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();

        var handler = new DeleteRoleCommandHandler(context);
        var command = new DeleteRoleCommand(role.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(role.Id, result.Id);
        Assert.Empty(await context.Roles.ToListAsync());
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUsersExist()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var role = new Role("Operator");
        await context.Roles.AddAsync(role);
        var user = new User("john", "john@factory.local", "hash", role.Id);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var handler = new DeleteRoleCommandHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new DeleteRoleCommand(role.Id), CancellationToken.None));
    }
}
