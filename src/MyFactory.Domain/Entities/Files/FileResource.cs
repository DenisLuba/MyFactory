using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Identity;

namespace MyFactory.Domain.Entities.Files;

/// <summary>
/// Aggregate root that stores metadata for uploaded files.
/// </summary>
public sealed class FileResource : BaseEntity
{
    public const int DescriptionMaxLength = 2000;

    private FileResource()
    {
    }

    public FileResource(
        string fileName,
        string storagePath,
        string contentType,
        long sizeBytes,
        Guid uploadedByUserId,
        DateTime uploadedAt,
        string? description = null)
    {
        Rename(fileName);
        MoveTo(storagePath);
        ChangeContentType(contentType);
        UpdateSize(sizeBytes);
        SetUploadedBy(uploadedByUserId);
        SetUploadedAt(uploadedAt);
        UpdateDescription(description);
    }

    public string FileName { get; private set; } = string.Empty;

    public string StoragePath { get; private set; } = string.Empty;

    public string ContentType { get; private set; } = string.Empty;

    public long SizeBytes { get; private set; }

    public Guid UploadedByUserId { get; private set; }

    public User? UploadedByUser { get; private set; }

    public DateTime UploadedAt { get; private set; }

    public string? Description { get; private set; }

    public void Rename(string newName)
    {
        Guard.AgainstNullOrWhiteSpace(newName, "File name is required.");
        FileName = newName.Trim();
    }

    public void MoveTo(string newStoragePath)
    {
        Guard.AgainstNullOrWhiteSpace(newStoragePath, "File path is required.");
        StoragePath = newStoragePath.Trim();
    }

    public void ChangeContentType(string newType)
    {
        Guard.AgainstNullOrWhiteSpace(newType, "Content type is required.");
        ContentType = newType.Trim();
    }

    public void UpdateSize(long sizeBytes)
    {
        if (sizeBytes < 0)
        {
            throw new DomainException("File size cannot be negative.");
        }

        SizeBytes = sizeBytes;
    }

    public void UpdateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            Description = null;
            return;
        }

        var trimmed = description.Trim();
        if (trimmed.Length > DescriptionMaxLength)
        {
            throw new DomainException($"Description cannot exceed {DescriptionMaxLength} characters.");
        }

        Description = trimmed;
    }

    private void SetUploadedBy(Guid uploadedByUserId)
    {
        Guard.AgainstEmptyGuid(uploadedByUserId, "Uploader id is required.");
        UploadedByUserId = uploadedByUserId;
    }

    private void SetUploadedAt(DateTime uploadedAt)
    {
        Guard.AgainstDefaultDate(uploadedAt, "Upload date is required.");
        UploadedAt = uploadedAt;
    }
}
