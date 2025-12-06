using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.Queries.GetMaterialById;

public sealed class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, MaterialDto>
{
    private readonly IApplicationDbContext _context;

    public GetMaterialByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MaterialDto> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
    {
        var material = await _context.Materials
            .AsNoTracking()
            .Include(entity => entity.MaterialType)
            .FirstOrDefaultAsync(entity => entity.Id == request.Id, cancellationToken);

        if (material is null)
        {
            throw new InvalidOperationException("Material not found.");
        }

        var typeDto = material.MaterialType is not null
            ? MaterialTypeDto.FromEntity(material.MaterialType)
            : new MaterialTypeDto(material.MaterialTypeId, string.Empty);

        return MaterialDto.FromEntity(material, typeDto);
    }
}
