using System;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Files.Queries.GetFileById;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Files;
using Xunit;

namespace MyFactory.Application.Tests.Features.Files;

public sealed class GetFileByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnDto_WhenFileExists()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var file = await SeedFileAsync(context);
        var handler = new GetFileByIdQueryHandler(context);

        var result = await handler.Handle(new GetFileByIdQuery(file.Id), CancellationToken.None);

        Assert.Equal(file.Id, result.Id);
        Assert.Equal(file.FileName, result.FileName);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenFileMissing()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var handler = new GetFileByIdQueryHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new GetFileByIdQuery(Guid.NewGuid()), CancellationToken.None));
    }

    private static async Task<FileResource> SeedFileAsync(TestApplicationDbContext context)
    {
        var file = new FileResource(
            "shipping-label.pdf",
            "/files/shipping-label.pdf",
            "application/pdf",
            512,
            Guid.NewGuid(),
            new DateTime(2025, 1, 8, 12, 0, 0, DateTimeKind.Utc),
            "Label");

        await context.FileResources.AddAsync(file);
        await context.SaveChangesAsync();
        return file;
    }
}
