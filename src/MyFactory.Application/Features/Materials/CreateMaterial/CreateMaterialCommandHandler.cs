using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.Features.Materials.CreateMaterial;

public sealed class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateMaterialCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        var exists = await _db.Materials
            .AsNoTracking()
            .AnyAsync(m => m.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (exists)
            throw new ValidationException($"Material with name '{request.Name}' already exists");

        var materialTypeExists = await _db.MaterialTypes
            .AsNoTracking()
            .AnyAsync(mt => mt.Id == request.MaterialTypeId, cancellationToken);
        if (!materialTypeExists)
            throw new NotFoundException("Material type not found");

        var unitExists = await _db.Units
            .AsNoTracking()
            .AnyAsync(u => u.Id == request.UnitId, cancellationToken);
        if (!unitExists)
            throw new NotFoundException("Unit not found");

        var material = new MaterialEntity(
            name: request.Name,
            materialTypeId: request.MaterialTypeId,
            unitId: request.UnitId,
            color: request.Color);

        await _db.Materials.AddAsync(material, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return material.Id;
    }
}
