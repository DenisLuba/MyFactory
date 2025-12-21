using System;
using MediatR;

namespace MyFactory.Application.OldFeatures.Files.Commands.DeleteFile;

public sealed record DeleteFileCommand(Guid FileId) : IRequest<bool>;
