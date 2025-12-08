using System;
using MediatR;
using MyFactory.Application.DTOs.Files;

namespace MyFactory.Application.Features.Files.Commands.UploadFile;

public sealed record UploadFileCommand(
    string FileName,
    string StoragePath,
    string ContentType,
    long SizeBytes,
    Guid UploadedByUserId,
    string? Description = null,
    DateTime? UploadedAt = null) : IRequest<FileResourceDto>;
