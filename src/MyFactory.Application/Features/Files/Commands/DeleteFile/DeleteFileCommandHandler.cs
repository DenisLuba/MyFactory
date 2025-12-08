using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Files.Commands.DeleteFile;

public sealed class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteFileCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _context.FileResources
            .FirstOrDefaultAsync(resource => resource.Id == request.FileId, cancellationToken)
            ?? throw new InvalidOperationException("File not found.");

        if (await HasReferences(request.FileId, cancellationToken))
        {
            throw new InvalidOperationException("File is referenced by existing records and cannot be deleted.");
        }

        _context.FileResources.Remove(file);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<bool> HasReferences(Guid fileId, CancellationToken cancellationToken)
    {
        // Extend this check when other aggregates reference FileResource.
        var advanceReportReference = await _context.AdvanceReports
            .AsNoTracking()
            .AnyAsync(report => report.FileId == fileId, cancellationToken);

        return advanceReportReference;
    }
}
