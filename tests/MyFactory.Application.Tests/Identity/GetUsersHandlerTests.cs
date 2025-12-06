using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Identity.Queries.GetUsers;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Identity;

namespace MyFactory.Application.Tests.Identity;

public class GetUsersHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAllUsers()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var role = new Role("RoleA");
        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();

        var roleId = role.Id;

        await context.Users.AddRangeAsync(
            new User("alpha", "alpha@example.com", "hash", roleId),
            new User("bravo", "bravo@example.com", "hash", roleId));
        await context.SaveChangesAsync();

        var handler = new GetUsersQueryHandler(ApplicationDbContextMockBuilder.Create(context).Object);

        var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
    }
}
