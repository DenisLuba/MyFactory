using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Units;

namespace MyFactory.Application.Features.Units.GetUnits;

public sealed class GetUnitsQueryHandler : IRequestHandler<GetUnitsQuery, IReadOnlyList<UnitDto>>
{
    private readonly IApplicationDbContext _db;

    public GetUnitsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<UnitDto>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        return await _db.Units
            .AsNoTracking()
            .Select(u => new UnitDto
            {
                Id = u.Id,
                Code = u.Code,
                Name = u.Name
            })
            .OrderBy(u => u.Name)
            .ToListAsync(cancellationToken);
    }
}
