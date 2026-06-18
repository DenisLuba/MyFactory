using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.MaterialTypes;

namespace MyFactory.Application.Features.MaterialTypes.GetMaterialTypes;

public sealed class GetMaterialTypesQueryHandler
    : IRequestHandler<GetMaterialTypesQuery, IReadOnlyList<MaterialTypeDto>>
{
    private readonly IApplicationDbContext _db;

    public GetMaterialTypesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<MaterialTypeDto>> Handle(
        GetMaterialTypesQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.MaterialTypes
            .AsNoTracking()
            .Where(t => !request.UsedOnly 
                || _db.Materials.Any(m => m.MaterialTypeId == t.Id))
            .Select(t => new MaterialTypeDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description
            })
            .ToListAsync(cancellationToken);
    }
}
