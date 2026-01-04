using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MyFactory.Application.Features.Identity.Commands.RefreshToken;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Tests.Identity;

public class RefreshTokenHandlerTests
{
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new();
    public RefreshTokenHandlerTests()
    {
        _jwtTokenServiceMock.Setup(service => service.RefreshTokensAsync("valid-token", It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, CancellationToken _) => new TokenResult("access", "refresh", DateTime.UtcNow.AddHours(1), Guid.Empty));
    }

    [Fact]
    public async Task Handle_ShouldReturnNewTokens_WhenRefreshTokenValid()
    {
        using var context = await BuildContextAsync();
        var userId = await context.Users.Select(user => user.Id).SingleAsync();
        _jwtTokenServiceMock.Setup(service => service.RefreshTokensAsync("valid-token", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TokenResult("access", "refresh", DateTime.UtcNow.AddHours(1), userId));

        var handler = new RefreshTokenCommandHandler(ApplicationDbContextMockBuilder.Create(context).Object, _jwtTokenServiceMock.Object);
        var command = new RefreshTokenCommand("valid-token");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("access", result.AccessToken);
        Assert.Equal("refresh", result.RefreshToken);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenRefreshTokenInvalid()
    {
        using var context = await BuildContextAsync();
        var handler = new RefreshTokenCommandHandler(ApplicationDbContextMockBuilder.Create(context).Object, _jwtTokenServiceMock.Object);
        _jwtTokenServiceMock.Setup(service => service.RefreshTokensAsync("invalid", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Invalid token"));

        var command = new RefreshTokenCommand("invalid");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserInactive()
    {
        using var context = await BuildContextAsync(isActive: false);
        var userId = await context.Users.Select(user => user.Id).SingleAsync();
        _jwtTokenServiceMock.Setup(service => service.RefreshTokensAsync("valid-token", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TokenResult("access", "refresh", DateTime.UtcNow.AddHours(1), userId));

        var handler = new RefreshTokenCommandHandler(ApplicationDbContextMockBuilder.Create(context).Object, _jwtTokenServiceMock.Object);
        var command = new RefreshTokenCommand("valid-token");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    private static async Task<TestApplicationDbContext> BuildContextAsync(bool isActive = true)
    {
        var context = TestApplicationDbContextFactory.Create();
        var role = new Role("Accountant");
        await context.Roles.AddAsync(role);

        var user = new User("jane", "jane@example.com", "hash", role.Id);
        if (!isActive)
        {
            user.Deactivate();
        }

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return context;
    }
}
