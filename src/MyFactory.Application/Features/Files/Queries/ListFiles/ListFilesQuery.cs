using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Files;

namespace MyFactory.Application.Features.Files.Queries.ListFiles;

public sealed record ListFilesQuery(
    Guid? UploadedByUserId = null,
    DateTime? UploadedFrom = null,
    DateTime? UploadedTo = null,
    string? ContentType = null) : IRequest<IReadOnlyList<FileResourceDto>>;
