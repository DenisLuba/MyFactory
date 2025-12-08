using System;
using MediatR;
using MyFactory.Application.DTOs.Files;

namespace MyFactory.Application.Features.Files.Commands.UpdateFileDescription;

public sealed record UpdateFileDescriptionCommand(Guid FileId, string Description) : IRequest<FileResourceDto>;
