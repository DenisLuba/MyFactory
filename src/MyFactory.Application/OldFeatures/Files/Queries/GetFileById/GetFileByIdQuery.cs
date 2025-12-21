using System;
using MediatR;
using MyFactory.Application.DTOs.Files;

namespace MyFactory.Application.OldFeatures.Files.Queries.GetFileById;

public sealed record GetFileByIdQuery(Guid FileId) : IRequest<FileResourceDto>;
