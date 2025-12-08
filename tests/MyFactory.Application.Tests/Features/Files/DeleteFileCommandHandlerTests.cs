using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Features.Files.Commands.DeleteFile;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Files;
using Xunit;

namespace MyFactory.Application.Tests.Features.Files;

public sealed class DeleteFileCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDelete_WhenFileExistsAndUnreferenced()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var file = await SeedFileAsync(context);
        var handler = new DeleteFileCommandHandler(context);
        var command = new DeleteFileCommand(file.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Empty(await context.FileResources.ToListAsync());
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenFileMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new DeleteFileCommandHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new DeleteFileCommand(Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenFileReferenced()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var file = await SeedFileAsync(context);

        var advance = new Advance(Guid.NewGuid(), 500m, new DateOnly(2025, 1, 1));
        advance.Issue();
        var report = advance.AddReport("Hotel", 200m, file.Id, new DateOnly(2025, 1, 15));
        await context.Advances.AddAsync(advance);
        await context.AdvanceReports.AddAsync(report);
        await context.SaveChangesAsync();

        var handler = new DeleteFileCommandHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new DeleteFileCommand(file.Id), CancellationToken.None));
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
