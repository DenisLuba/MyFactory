using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.DTOs.Warehouses;

namespace MyFactory.Application.Features.Warehouses.GetWarehouseInfo;

public sealed class GetWarehouseInfoQueryHandler
    : IRequestHandler<GetWarehouseInfoQuery, WarehouseInfoDto>
{
    private readonly IApplicationDbContext _db;

    public GetWarehouseInfoQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<WarehouseInfoDto> Handle(
        GetWarehouseInfoQuery request,
        CancellationToken cancellationToken)
    {
        var warehouse = await _db.Warehouses
            .AsNoTracking()
            .Where(x => x.Id == request.WarehouseId)
            .Select(x => new WarehouseInfoDto
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        return warehouse;
    }
}
