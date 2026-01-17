using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Warehouses.RemoveWarehouse;

public class RemoveWarehouseCommandHandler : IRequestHandler<RemoveWarehouseCommand>
{
    private readonly IApplicationDbContext _context;

    public RemoveWarehouseCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
    }

    public async Task Handle(RemoveWarehouseCommand request, CancellationToken ct)
    {
        var warehouse = await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Id == request.WarehouseId, ct)
            ?? throw new NotFoundException("Warehouse not found.");

        _context.Warehouses.Remove(warehouse);

        await _context.SaveChangesAsync(ct);
    }
}
