using System;
using MediatR;

namespace MyFactory.Application.Features.Files.Commands.DeleteFile;

public sealed record DeleteFileCommand(Guid FileId) : IRequest<bool>;
