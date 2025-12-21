using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.OldFeatures.Materials.Commands.UpdateMaterial;

public sealed class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand, MaterialDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MaterialDto> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _context.Materials
            .FirstOrDefaultAsync(entity => entity.Id == request.Id, cancellationToken);

        if (material is null)
        {
            throw new InvalidOperationException("Material not found.");
        }

        if (request.Name is { } rawName)
        {
            var name = rawName.Trim();
            var duplicate = await _context.Materials
                .AsNoTracking()
                .AnyAsync(entity => entity.Id != material.Id && entity.Name == name, cancellationToken);

            if (duplicate)
            {
                throw new InvalidOperationException("Material name already exists.");
            }

            material.UpdateName(name);
        }

        if (request.Unit is { } rawUnit)
        {
            material.ChangeUnit(rawUnit.Trim());
        }

        if (request.MaterialTypeId is Guid materialTypeId)
        {
            var materialType = await _context.MaterialTypes
                .FirstOrDefaultAsync(type => type.Id == materialTypeId, cancellationToken);

            if (materialType is null)
            {
                throw new InvalidOperationException("Material type does not exist.");
            }

            material.ChangeType(materialTypeId);
        }

        if (request.IsActive.HasValue)
        {
            var shouldBeActive = request.IsActive.Value;
            if (!shouldBeActive && material.IsActive)
            {
                material.Deactivate();
            }
            else if (shouldBeActive && !material.IsActive)
            {
                throw new InvalidOperationException("Material cannot be reactivated once inactive.");
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        var currentType = await _context.MaterialTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(type => type.Id == material.MaterialTypeId, cancellationToken)
            ?? throw new InvalidOperationException("Material type does not exist.");

        return MaterialDto.FromEntity(material, MaterialTypeDto.FromEntity(currentType));
    }
}
