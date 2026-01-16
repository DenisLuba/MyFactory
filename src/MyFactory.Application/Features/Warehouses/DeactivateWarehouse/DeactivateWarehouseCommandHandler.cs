using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed class DeactivateWarehouseCommandHandler
    : IRequestHandler<DeactivateWarehouseCommand>
{
    private readonly IApplicationDbContext _db;

    public DeactivateWarehouseCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        DeactivateWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        var warehouse = await _db.Warehouses
            .FirstOrDefaultAsync(
                w => w.Id == request.WarehouseId,
                cancellationToken)
            ?? throw new NotFoundException(
                $"Warehouse with Id {request.WarehouseId} not found");

        _db.Warehouses.Remove(warehouse);

        await _db.SaveChangesAsync(cancellationToken);
    }
}