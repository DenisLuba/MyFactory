using System;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Application.Features.Files.Queries.ListFiles;
using MyFactory.Application.Tests.Common;
using MyFactory.Domain.Entities.Files;
using Xunit;

namespace MyFactory.Application.Tests.Features.Files;

public sealed class ListFilesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldFilterByUploaderAndDateRange()
    {
        using var context = TestApplicationDbContextFactory.Create();
        var uploader = Guid.NewGuid();
        var anotherUploader = Guid.NewGuid();

        var fileA = new FileResource(
            "a.pdf",
            "/files/a.pdf",
            "application/pdf",
            100,
            uploader,
            new DateTime(2025, 1, 5, 10, 0, 0, DateTimeKind.Utc),
            "A");
        var fileB = new FileResource(
            "b.pdf",
            "/files/b.pdf",
            "application/pdf",
            200,
            uploader,
            new DateTime(2025, 1, 15, 10, 0, 0, DateTimeKind.Utc),
            "B");
        var fileC = new FileResource(
            "c.docx",
            "/files/c.docx",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            300,
            anotherUploader,
            new DateTime(2025, 1, 20, 10, 0, 0, DateTimeKind.Utc),
            "C");

        await context.FileResources.AddRangeAsync(fileA, fileB, fileC);
        await context.SaveChangesAsync();

        var handler = new ListFilesQueryHandler(context);
        var query = new ListFilesQuery(
            UploadedByUserId: uploader,
            UploadedFrom: new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UploadedTo: new DateTime(2025, 1, 31, 23, 59, 59, DateTimeKind.Utc),
            ContentType: "application/pdf");

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("b.pdf", result[0].FileName);
        Assert.Equal("a.pdf", result[1].FileName);
    }

    [Fact]
    public async Task Handle_ShouldReturnAll_WhenNoFilters()
    {
        using var context = TestApplicationDbContextFactory.Create();
        await context.FileResources.AddRangeAsync(
            new FileResource("a.pdf", "/files/a.pdf", "application/pdf", 10, Guid.NewGuid(), DateTime.UtcNow, null),
            new FileResource("b.pdf", "/files/b.pdf", "application/pdf", 10, Guid.NewGuid(), DateTime.UtcNow, null));
        await context.SaveChangesAsync();

        var handler = new ListFilesQueryHandler(context);
        var result = await handler.Handle(new ListFilesQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
    }
}
