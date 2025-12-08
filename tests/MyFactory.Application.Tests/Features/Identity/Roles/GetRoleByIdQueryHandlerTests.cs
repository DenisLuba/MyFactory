using System;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Identity.Roles.Queries.GetRoleById;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Identity;
using Xunit;

namespace MyFactory.Application.Tests.Features.Identity.Roles;

public sealed class GetRoleByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnRole()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var role = new Role("Operator", "Handles operations");
        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();

        var handler = new GetRoleByIdQueryHandler(context);
        var query = new GetRoleByIdQuery(role.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(role.Id, result.Id);
        Assert.Equal("Handles operations", result.Description);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenRoleMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new GetRoleByIdQueryHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new GetRoleByIdQuery(Guid.NewGuid()), CancellationToken.None));
    }
}
