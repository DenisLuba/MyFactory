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

        if (material is null)
            throw new NotFoundException($"Material with Id {request.MaterialId} not found");

        _db.Materials.Remove(material);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
