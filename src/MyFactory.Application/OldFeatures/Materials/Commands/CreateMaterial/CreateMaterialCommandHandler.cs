using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.OldFeatures.Materials.Commands.CreateMaterial;

public sealed class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, MaterialDto>
{
    private readonly IApplicationDbContext _context;

    public CreateMaterialCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MaterialDto> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();
        var unit = request.Unit.Trim();

        var duplicate = await _context.Materials
            .AsNoTracking()
            .AnyAsync(material => material.Name == name, cancellationToken);

        if (duplicate)
        {
            throw new InvalidOperationException("Material name already exists.");
        }

        var materialType = await _context.MaterialTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(type => type.Id == request.MaterialTypeId, cancellationToken);

        if (materialType is null)
        {
            throw new InvalidOperationException("Material type does not exist.");
        }

        var material = new Material(name, request.MaterialTypeId, unit);

        await _context.Materials.AddAsync(material, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return MaterialDto.FromEntity(material, MaterialTypeDto.FromEntity(materialType));
    }
}
