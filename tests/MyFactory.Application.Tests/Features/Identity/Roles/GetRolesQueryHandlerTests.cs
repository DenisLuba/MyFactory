using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Identity.Roles.Queries.GetRoles;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Identity;
using Xunit;

namespace MyFactory.Application.Tests.Features.Identity.Roles;

public sealed class GetRolesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnOrderedRoles()
    {
        using var context = TestApplicationDbContextFactory.Create();
        await context.Roles.AddRangeAsync(new Role("Supervisor"), new Role("Operator"));
        await context.SaveChangesAsync();

        var handler = new GetRolesQueryHandler(context);

        var result = await handler.Handle(new GetRolesQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(new[] { "Operator", "Supervisor" }, result.Select(r => r.Name).ToArray());
    }
}
