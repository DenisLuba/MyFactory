using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Materials.RemoveMaterial;

public sealed class RemoveMaterialCommandHandler : IRequestHandler<RemoveMaterialCommand>
{
    private readonly IApplicationDbContext _db;

    public RemoveMaterialCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(RemoveMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _db.Materials
            .FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken);

        var images = await _db.MaterialImages
            .Where(i => i.MaterialId == request.MaterialId)
            .ToListAsync(cancellationToken);

        if (material is null)
            throw new NotFoundException($"Material with Id {request.MaterialId} not found");

        if (images is not null && images.Count > 0)
        {
            _db.MaterialImages.RemoveRange(images);
        }

        _db.Materials.Remove(material);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
