using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.MaterialTypes.DeactivateMaterialType;

public sealed class DeactivateMaterialTypeCommandHandler : IRequestHandler<DeactivateMaterialTypeCommand>
{
    private readonly IApplicationDbContext _db;

    public DeactivateMaterialTypeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(DeactivateMaterialTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.MaterialTypes
            .FirstOrDefaultAsync(x => x.Id == request.MaterialTypeId, cancellationToken)
            ?? throw new NotFoundException("Material type not found");

        var hasMaterials = await _db.Materials
            .AsNoTracking()
            .AnyAsync(x => x.MaterialTypeId == entity.Id, cancellationToken);

        if (hasMaterials)
            throw new DomainException("Cannot delete material type while materials reference it.");

        _db.MaterialTypes.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
