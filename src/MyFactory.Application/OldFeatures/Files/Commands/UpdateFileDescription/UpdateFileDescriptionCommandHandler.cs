using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Files;

namespace MyFactory.Application.OldFeatures.Files.Commands.UpdateFileDescription;

public sealed class UpdateFileDescriptionCommandHandler : IRequestHandler<UpdateFileDescriptionCommand, FileResourceDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateFileDescriptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FileResourceDto> Handle(UpdateFileDescriptionCommand request, CancellationToken cancellationToken)
    {
        var file = await _context.FileResources
            .FirstOrDefaultAsync(resource => resource.Id == request.FileId, cancellationToken)
            ?? throw new InvalidOperationException("File not found.");

        file.UpdateDescription(request.Description);
        await _context.SaveChangesAsync(cancellationToken);

        return FileResourceDto.FromEntity(file);
    }
}
