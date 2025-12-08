using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Files.Commands.UploadFile;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Identity;
using Xunit;

namespace MyFactory.Application.Tests.Features.Files;

public sealed class UploadFileCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPersistMetadata()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var user = await SeedUserAsync(context);
        var handler = new UploadFileCommandHandler(context);
        var timestamp = new DateTime(2025, 1, 10, 12, 0, 0, DateTimeKind.Utc);

        var command = new UploadFileCommand(
            "advance-report.pdf",
            "/files/2025/01/advance-report.pdf",
            "application/pdf",
            4_096,
            user.Id,
            "Advance report",
            timestamp);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("advance-report.pdf", result.FileName);
        Assert.Equal(user.Id, result.UploadedByUserId);
        Assert.Equal(timestamp, result.UploadedAt);

        var stored = await context.FileResources.SingleAsync();
        Assert.Equal(command.StoragePath, stored.StoragePath);
        Assert.Equal(command.Description, stored.Description);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenFilenameInvalid()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var user = await SeedUserAsync(context);
        var handler = new UploadFileCommandHandler(context);
        var command = new UploadFileCommand(
            string.Empty,
            "/files/empty.pdf",
            "application/pdf",
            10,
            user.Id);

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenSizeNegative()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var user = await SeedUserAsync(context);
        var handler = new UploadFileCommandHandler(context);
        var command = new UploadFileCommand(
            "bad-size.pdf",
            "/files/bad-size.pdf",
            "application/pdf",
            -1,
            user.Id);

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new UploadFileCommandHandler(context);
        var command = new UploadFileCommand(
            "report.pdf",
            "/files/report.pdf",
            "application/pdf",
            256,
            Guid.NewGuid());

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    private static async Task<User> SeedUserAsync(TestApplicationDbContext context)
    {
        var role = new Role("Operator");
        await context.Roles.AddAsync(role);

        var user = new User("jdoe", "jdoe@example.com", "hash", role.Id, new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }
}
