using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed class UpdateWarehouseCommandHandler
    : IRequestHandler<UpdateWarehouseCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateWarehouseCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        UpdateWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        var warehouse = await _db.Warehouses
            .FirstOrDefaultAsync(
                w => w.Id == request.WarehouseId,
                cancellationToken)
            ?? throw new NotFoundException(
                $"Warehouse with Id {request.WarehouseId} not found");

        warehouse.Update(request.Name, request.Type);

        await _db.SaveChangesAsync(cancellationToken);
    }
}