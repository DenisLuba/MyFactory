using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.UpdateWarehouse;

public sealed class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, WarehouseDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateWarehouseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WarehouseDto> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await _context.Warehouses
            .FirstOrDefaultAsync(entity => entity.Id == request.Id, cancellationToken);

        if (warehouse is null)
        {
            throw new InvalidOperationException("Warehouse not found.");
        }

        var targetName = request.Name?.Trim() ?? warehouse.Name;
        var targetLocation = request.Location?.Trim() ?? warehouse.Location;

        var exists = await _context.Warehouses
            .AsNoTracking()
            .AnyAsync(entity => entity.Id != warehouse.Id && entity.Name == targetName && entity.Location == targetLocation, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Warehouse with the same name and location already exists.");
        }

        if (request.Name is not null)
        {
            warehouse.Rename(targetName);
        }

        if (request.Type is { } rawType)
        {
            warehouse.ChangeType(rawType.Trim());
        }

        if (request.Location is not null)
        {
            warehouse.ChangeLocation(targetLocation);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return WarehouseDto.FromEntity(warehouse);
    }
}
