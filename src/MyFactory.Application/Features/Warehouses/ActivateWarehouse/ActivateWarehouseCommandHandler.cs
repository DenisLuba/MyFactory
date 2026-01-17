using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Warehouses.ActivateWarehouse;

public class ActivateWarehouseCommandHandler : IRequestHandler<ActivateWarehouseCommand>
{
    private readonly IApplicationDbContext _context;

    public ActivateWarehouseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActivateWarehouseCommand request, CancellationToken ct)
    { 
        var warehouse = await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Id == request.WarehouseId) 
            ?? throw new NotFoundException("Warehouse not found.");

        warehouse.Activate();

        await _context.SaveChangesAsync(ct);
    }
}
