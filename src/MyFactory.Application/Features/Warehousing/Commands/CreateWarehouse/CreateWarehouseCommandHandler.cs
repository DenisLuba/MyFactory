using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.CreateWarehouse;

public sealed class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, WarehouseDto>
{
    private readonly IApplicationDbContext _context;

    public CreateWarehouseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WarehouseDto> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();
        var type = request.Type.Trim();
        var location = request.Location.Trim();

        var exists = await _context.Warehouses
            .AsNoTracking()
            .AnyAsync(warehouse => warehouse.Name == name && warehouse.Location == location, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Warehouse with the same name and location already exists.");
        }

        var warehouse = new Warehouse(name, type, location);
        await _context.Warehouses.AddAsync(warehouse, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return WarehouseDto.FromEntity(warehouse);
    }
}
