using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.MaterialTypes;

namespace MyFactory.Application.Features.MaterialTypes.GetMaterialTypeDetails;

public sealed class GetMaterialTypeDetailsQueryHandler
    : IRequestHandler<GetMaterialTypeDetailsQuery, MaterialTypeDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetMaterialTypeDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<MaterialTypeDetailsDto> Handle(
        GetMaterialTypeDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _db.MaterialTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.MaterialTypeId, cancellationToken)
            ?? throw new NotFoundException("Material type not found");

        return new MaterialTypeDetailsDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}
