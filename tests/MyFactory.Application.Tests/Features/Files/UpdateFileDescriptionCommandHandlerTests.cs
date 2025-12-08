using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Files.Commands.UpdateFileDescription;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Files;
using Xunit;

namespace MyFactory.Application.Tests.Features.Files;

public sealed class UpdateFileDescriptionCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateDescription()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var file = await SeedFileAsync(context);
        var handler = new UpdateFileDescriptionCommandHandler(context);

        var result = await handler.Handle(
            new UpdateFileDescriptionCommand(file.Id, "Updated description"),
            CancellationToken.None);

        Assert.Equal("Updated description", result.Description);

        var stored = await context.FileResources.SingleAsync();
        Assert.Equal("Updated description", stored.Description);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenDescriptionTooLong()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var file = await SeedFileAsync(context);
        var handler = new UpdateFileDescriptionCommandHandler(context);
        var newDescription = new string('a', FileResource.DescriptionMaxLength + 1);

        await Assert.ThrowsAsync<DomainException>(() =>
            handler.Handle(new UpdateFileDescriptionCommand(file.Id, newDescription), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenFileMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new UpdateFileDescriptionCommandHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new UpdateFileDescriptionCommand(Guid.NewGuid(), "desc"), CancellationToken.None));
    }

    private static async Task<FileResource> SeedFileAsync(TestApplicationDbContext context)
    {
        var file = new FileResource(
            "receipt.pdf",
            "/files/receipt.pdf",
            "application/pdf",
            1_024,
            Guid.NewGuid(),
            new DateTime(2025, 1, 5, 10, 0, 0, DateTimeKind.Utc),
            "Travel receipt");

        await context.FileResources.AddAsync(file);
        await context.SaveChangesAsync();
        return file;
    }
}
