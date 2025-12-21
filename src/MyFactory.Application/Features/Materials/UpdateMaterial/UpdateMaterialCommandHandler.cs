using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Materials.UpdateMaterial;

public sealed class UpdateMaterialCommandHandler
    : IRequestHandler<UpdateMaterialCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateMaterialCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        UpdateMaterialCommand request,
        CancellationToken cancellationToken)
    {
        var material = await _db.Materials
            .FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken);

        if (material is null)
            throw new NotFoundException(
                $"Material with Id {request.MaterialId} not found");

        material.Update(
            name: request.Name,
            materialTypeId: request.MaterialTypeId,
            unitId: request.UnitId,
            color: request.Color
        );

        await _db.SaveChangesAsync(cancellationToken);
    }
}
