using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MyFactory.Application.Features.Identity.Commands.Login;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Tests.Identity;

public class LoginHandlerTests
{
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new();
    public LoginHandlerTests()
    {
        _passwordHasherMock.Setup(hasher => hasher.VerifyAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _jwtTokenServiceMock.Setup(service => service.GenerateTokensAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User user, CancellationToken _) => new TokenResult("access", "refresh", DateTime.UtcNow.AddHours(1), user.Id));
    }

    [Fact]
    public async Task Handle_ShouldReturnTokens_WhenCredentialsValid()
    {
        using var context = await BuildContextAsync(isActive: true);
        var handler = new LoginCommandHandler(ApplicationDbContextMockBuilder.Create(context).Object, _passwordHasherMock.Object, _jwtTokenServiceMock.Object);
        var command = new LoginCommand("john", "password");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("access", result.AccessToken);
        Assert.Equal("refresh", result.RefreshToken);
        _jwtTokenServiceMock.Verify(service => service.GenerateTokensAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenPasswordInvalid()
    {
        using var context = await BuildContextAsync(isActive: true);
        var handler = new LoginCommandHandler(ApplicationDbContextMockBuilder.Create(context).Object, _passwordHasherMock.Object, _jwtTokenServiceMock.Object);
        _passwordHasherMock.Setup(hasher => hasher.VerifyAsync("password", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new LoginCommand("john", "password");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        _jwtTokenServiceMock.Verify(service => service.GenerateTokensAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserInactive()
    {
        using var context = await BuildContextAsync(isActive: false);
        var handler = new LoginCommandHandler(ApplicationDbContextMockBuilder.Create(context).Object, _passwordHasherMock.Object, _jwtTokenServiceMock.Object);
        var command = new LoginCommand("john", "password");

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    private static async Task<TestApplicationDbContext> BuildContextAsync(bool isActive)
    {
        var context = TestApplicationDbContextFactory.Create();
        var role = new Role("Admin");
        await context.Roles.AddAsync(role);

        var user = new User("john", "john@example.com", "hashed", role.Id);
        if (!isActive)
        {
            user.Deactivate();
        }

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return context;
    }
}
