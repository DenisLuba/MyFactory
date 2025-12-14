using System;
using MyFactory.Domain.Entities.Files;
using MyFactory.Domain.Exceptions;
using Xunit;

namespace MyFactory.Domain.Tests.Files;

public class FileResourceTests
{
    private static FileResource CreateSample()
        => new(
            "report.pdf",
            "/files/2025/01/report.pdf",
            "application/pdf",
            12_345,
            Guid.NewGuid(),
            new DateTime(2025, 1, 5, 10, 30, 0, DateTimeKind.Utc),
            "Quarterly advance report");

    [Fact]
    public void Constructor_WithValidData_CreatesEntity()
    {
        var uploader = Guid.NewGuid();
        var uploadedAt = new DateTime(2025, 2, 1, 12, 0, 0, DateTimeKind.Utc);
        var file = new FileResource("photo.jpg", "/uploads/photo.jpg", "image/jpeg", 2_048, uploader, uploadedAt, "sample");

        Assert.Equal("photo.jpg", file.FileName);
        Assert.Equal("/uploads/photo.jpg", file.FilePath);
        Assert.Equal("image/jpeg", file.ContentType);
        Assert.Equal(2_048, file.SizeBytes);
        Assert.Equal(uploader, file.UploadedByUserId);
        Assert.Equal(uploadedAt, file.UploadedAt);
    }

    [Fact]
    public void Constructor_WithEmptyFilename_Throws()
    {
        Assert.Throws<DomainException>(() =>
            new FileResource(string.Empty, "/path", "text/plain", 1, Guid.NewGuid(), new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
    }

    [Fact]
    public void Rename_WithValidName_UpdatesValue()
    {
        var file = CreateSample();
        file.Rename("new-name.pdf");

        Assert.Equal("new-name.pdf", file.FileName);
    }

    [Fact]
    public void Rename_WithEmptyName_Throws()
    {
        var file = CreateSample();

        Assert.Throws<DomainException>(() => file.Rename(""));
    }

    [Fact]
    public void MoveTo_WithValidPath_UpdatesValue()
    {
        var file = CreateSample();
        file.MoveTo("/archive/report.pdf");

        Assert.Equal("/archive/report.pdf", file.FilePath);
    }

    [Fact]
    public void MoveTo_WithEmptyPath_Throws()
    {
        var file = CreateSample();

        Assert.Throws<DomainException>(() => file.MoveTo(""));
    }
}
