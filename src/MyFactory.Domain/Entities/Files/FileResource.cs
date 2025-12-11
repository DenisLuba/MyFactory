using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Identity;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Domain.Entities.Files;

/// <summary>
/// Aggregate root that stores metadata for uploaded files. Binary content must be kept in external storage.
/// </summary>
public sealed class FileResource : BaseEntity
{
    public const int FilenameMaxLength = 260; // typical filesystem limit
    public const int PathMaxLength = 2048;
    public const int ContentTypeMaxLength = 200;
    public const int DescriptionMaxLength = 1000;
    public const int ChecksumHexLength = 64; // SHA-256 hex length

    private readonly List<AdvanceReport> _advanceReports = new();

    private FileResource()
    {
    }

    private FileResource(string filename, string path, string contentType, long size, Guid uploadedById, DateTime uploadedAt, string? description = null, string? checksum = null)
    {
        UpdateFilename(filename);
        UpdatePath(path);
        UpdateContentType(contentType);
        UpdateSize(size);
        SetUploadedBy(uploadedById);
        SetUploadedAt(uploadedAt);
        UpdateDescription(description);
        UpdateChecksum(checksum);
    }

    /// <summary>
    /// Create file metadata record. Does not upload or store binary content.
    /// TODO: Application layer must upload file to storage and coordinate with this metadata (path must be a storage locator).
    /// </summary>
    public static FileResource Create(string filename, string path, string contentType, long size, Guid uploadedById, DateTime uploadedAtUtc, string? description = null, string? checksum = null)
    {
        // Ensure uploadedAt is not default and normalize to UTC
        Guard.AgainstDefaultDate(uploadedAtUtc, nameof(uploadedAtUtc));
        var utc = uploadedAtUtc.Kind == DateTimeKind.Utc ? uploadedAtUtc : uploadedAtUtc.ToUniversalTime();

        var f = new FileResource(filename, path, contentType, size, uploadedById, utc, description, checksum);
        // TODO: consider raising FileResourceUploaded domain event here: f.AddDomainEvent(new FileResourceUploaded(f.Id, utc));
        return f;
    }

    // Properties mapped to ERD
    public string Filename { get; private set; } = string.Empty;

    /// <summary>
    /// Storage locator: could be blob key, URL or provider-specific path. Do not assume local filesystem.
    /// </summary>
    public string StoragePath { get; private set; } = string.Empty;

    public string ContentType { get; private set; } = string.Empty;

    // bigint in DB
    public long Size { get; private set; }

    public Guid UploadedById { get; private set; }

    // ERD defines USERS as uploader; map to User entity (internal setter for ORM).
    public User? UploadedBy { get; internal set; }

    // timestamp in DB; stored in UTC
    public DateTime UploadedAt { get; private set; }

    public string? Description { get; private set; }

    /// <summary>
    /// SHA-256 checksum represented as lower-case hex (64 chars). Optional; compute in storage provider and pass to domain metadata.
    /// </summary>
    public string? Checksum { get; private set; }

    // Relationship: FILES ||--o{ ADVANCE_REPORTS : attach
    public IReadOnlyCollection<AdvanceReport> AdvanceReports => _advanceReports.AsReadOnly();

    // Update methods
    public void UpdateFilename(string filename)
    {
        Guard.AgainstNullOrWhiteSpace(filename, nameof(filename));
        var trimmed = filename.Trim();
        if (trimmed.Length > FilenameMaxLength)
            throw new DomainException($"Filename cannot exceed {FilenameMaxLength} characters.");

        // Basic sanitize: disallow directory traversal chars in filename
        if (trimmed.Contains("..") || System.IO.Path.GetFileName(trimmed) != trimmed)
            throw new DomainException("Invalid file name.");

        Filename = trimmed;
    }

    public void UpdatePath(string path)
    {
        Guard.AgainstNullOrWhiteSpace(path, nameof(path));
        var trimmed = path.Trim();
        if (trimmed.Length > PathMaxLength)
            throw new DomainException($"Path cannot exceed {PathMaxLength} characters.");

        // StoragePath is treated as an opaque storage locator (blob key, provider URL, signed URL, etc.).
        // Do NOT attempt complex provider-specific validation in the domain model; perform strict
        // validation/sanitization in the application/storage service (check scheme, allowed prefixes,
        // signed-url patterns, etc.). The domain enforces only basic non-emptiness and length here.
        StoragePath = trimmed;
    }

    public void UpdateContentType(string contentType)
    {
        Guard.AgainstNullOrWhiteSpace(contentType, nameof(contentType));
        var trimmed = contentType.Trim();
        if (trimmed.Length > ContentTypeMaxLength)
            throw new DomainException($"Content type cannot exceed {ContentTypeMaxLength} characters.");

        ContentType = trimmed;
    }

    public void UpdateSize(long size)
    {
        Guard.AgainstNegative(size, nameof(size));
        Size = size;
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
            throw new DomainException($"Description cannot exceed {DescriptionMaxLength} characters.");

        Description = trimmed;
    }

    /// <summary>
    /// Update optional checksum. Accepts null to clear.
    /// Validates hex-encoded SHA-256 (64 hex chars).
    /// </summary>
    public void UpdateChecksum(string? checksum)
    {
        if (string.IsNullOrWhiteSpace(checksum))
        {
            Checksum = null;
            return;
        }

        var trimmed = checksum.Trim();
        if (trimmed.Length != ChecksumHexLength)
            throw new DomainException($"Checksum must be {ChecksumHexLength} hex characters (SHA-256).");

        // Validate hex
        if (!Regex.IsMatch(trimmed, "^[A-Fa-f0-9]{64}$"))
            throw new DomainException("Checksum must be a valid hex string.");

        Checksum = trimmed.ToLowerInvariant();
    }

    private void SetUploadedBy(Guid uploadedById)
    {
        Guard.AgainstEmptyGuid(uploadedById, nameof(uploadedById));
        UploadedById = uploadedById;
    }

    private void SetUploadedAt(DateTime uploadedAtUtc)
    {
        Guard.AgainstDefaultDate(uploadedAtUtc, nameof(uploadedAtUtc));
        UploadedAt = uploadedAtUtc.Kind == DateTimeKind.Utc ? uploadedAtUtc : uploadedAtUtc.ToUniversalTime();
    }

    // Attach/detach AdvanceReport
    internal void AttachAdvanceReport(AdvanceReport report)
    {
        Guard.AgainstNull(report, nameof(report));
        if (report.FileId != Id)
            throw new DomainException("Report does not reference this file resource.");

        if (_advanceReports.Any(r => r.Id == report.Id)) return;
        report.File = this;
        _advanceReports.Add(report);
    }

    internal void DetachAdvanceReport(AdvanceReport report)
    {
        Guard.AgainstNull(report, nameof(report));
        var idx = _advanceReports.FindIndex(r => r.Id == report.Id);
        if (idx == -1) return;
        _advanceReports.RemoveAt(idx);
        report.File = null;
    }

    // Security/storage notes:
    // - Do not store binary content in domain. Store metadata only and upload file using storage provider in application layer.
    // - Path should be treated as a storage locator (blob key or provider URL). Sanitize inputs and avoid client-controlled paths.
    // - Checksum (SHA-256) is optional metadata to verify integrity; compute in storage provider and pass to domain metadata.
    // - Ensure UploadedAt is stored in UTC and map DateTime.Kind/precision in ORM.
    // - Repository/UnitOfWork must dispatch domain events after successful commit (TODO).
}
