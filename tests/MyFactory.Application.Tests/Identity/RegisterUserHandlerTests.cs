using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MyFactory.Application.Features.Identity.Commands.RegisterUser;
using MyFactory.Application.Interfaces.Auth;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Identity;
using MyFactory.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MyFactory.Application.Tests.Identity;

public class RegisterUserHandlerTests
{
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();

    public RegisterUserHandlerTests()
    {
        _passwordHasherMock.Setup(hasher => hasher.HashAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("hashed-password");
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenDataIsValid()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var role = new Role("Admin");
        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();

        var handler = new RegisterUserCommandHandler(ApplicationDbContextMockBuilder.Create(context).Object, _passwordHasherMock.Object);
        var command = new RegisterUserCommand("john_doe", "john@example.com", "P@ssw0rd", role.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("john_doe", result.Username);
        Assert.Equal(1, await context.Users.CountAsync());
        _passwordHasherMock.Verify(hasher => hasher.HashAsync("P@ssw0rd", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUsernameExists()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var role = new Role("Admin");
        await context.Roles.AddAsync(role);
        await context.Users.AddAsync(new User("john_doe", "existing@example.com", "hash", role.Id));
        await context.SaveChangesAsync();

        var handler = new RegisterUserCommandHandler(ApplicationDbContextMockBuilder.Create(context).Object, _passwordHasherMock.Object);
        var command = new RegisterUserCommand("john_doe", "new@example.com", "password", role.Id);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}
