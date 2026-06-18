using MediatR;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed class CreateWarehouseCommandHandler
    : IRequestHandler<CreateWarehouseCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateWarehouseCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        CreateWarehouseCommand request,
        CancellationToken cancellationToken)
    {
        var warehouse = new WarehouseEntity(
            request.Name,
            request.Type);

        _db.Warehouses.Add(warehouse);

        await _db.SaveChangesAsync(cancellationToken);

        return warehouse.Id;
    }
}