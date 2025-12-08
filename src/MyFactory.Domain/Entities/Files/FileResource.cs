using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Identity;

namespace MyFactory.Domain.Entities.Files;

/// <summary>
/// Aggregate root that stores metadata for uploaded files.
/// </summary>
public sealed class FileResource : BaseEntity
{
    private FileResource()
    {
    }

    public FileResource(string fileName, string path, string contentType, long size, Guid uploadedBy, DateTime uploadedAt)
    {
        Rename(fileName);
        MoveTo(path);
        ChangeContentType(contentType);
        UpdateSize(size);
        SetUploadedBy(uploadedBy);
        SetUploadedAt(uploadedAt);
    }

    public string FileName { get; private set; } = string.Empty;

    public string Path { get; private set; } = string.Empty;

    public string ContentType { get; private set; } = string.Empty;

    public long Size { get; private set; }

    public Guid UploadedBy { get; private set; }

    public User? UploadedByUser { get; private set; }

    public DateTime UploadedAt { get; private set; }

    public void Rename(string newName)
    {
        Guard.AgainstNullOrWhiteSpace(newName, "File name is required.");
        FileName = newName.Trim();
    }

    public void MoveTo(string newPath)
    {
        Guard.AgainstNullOrWhiteSpace(newPath, "File path is required.");
        Path = newPath.Trim();
    }

    public void ChangeContentType(string newType)
    {
        Guard.AgainstNullOrWhiteSpace(newType, "Content type is required.");
        ContentType = newType.Trim();
    }

    public void UpdateSize(long size)
    {
        if (size < 0)
        {
            throw new DomainException("File size cannot be negative.");
        }

        Size = size;
    }

    private void SetUploadedBy(Guid uploadedBy)
    {
        Guard.AgainstEmptyGuid(uploadedBy, "Uploader id is required.");
        UploadedBy = uploadedBy;
    }

    private void SetUploadedAt(DateTime uploadedAt)
    {
        Guard.AgainstDefaultDate(uploadedAt, "Upload date is required.");
        UploadedAt = uploadedAt;
    }
}
