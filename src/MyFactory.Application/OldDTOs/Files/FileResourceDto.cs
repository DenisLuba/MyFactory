using System;
using MyFactory.Domain.Entities.Files;

namespace MyFactory.Application.OldDTOs.Files;

public sealed record FileResourceDto(
    Guid Id,
    string FileName,
    string StoragePath,
    string ContentType,
    long SizeBytes,
    string? Description,
    DateTime UploadedAt,
    Guid UploadedByUserId)
{
    public static FileResourceDto FromEntity(FileResource file)
        => new(
            file.Id,
            file.FileName,
            file.StoragePath,
            file.ContentType,
            file.SizeBytes,
            file.Description,
            file.UploadedAt,
            file.UploadedByUserId);
}
