using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.OldFeatures.Materials.Queries.GetMaterials;

public sealed class GetMaterialsQueryHandler : IRequestHandler<GetMaterialsQuery, IReadOnlyCollection<MaterialDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMaterialsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<MaterialDto>> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
    {
        var materials = await _context.Materials
            .AsNoTracking()
            .Include(material => material.MaterialType)
            .OrderBy(material => material.Name)
            .ToListAsync(cancellationToken);

        return materials
            .Select(material =>
            {
                var typeDto = material.MaterialType is not null
                    ? MaterialTypeDto.FromEntity(material.MaterialType)
                    : new MaterialTypeDto(material.MaterialTypeId, string.Empty);

                return MaterialDto.FromEntity(material, typeDto);
            })
            .ToList();
    }
}
