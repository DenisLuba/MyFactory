using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.CreateProductionOrder;

public sealed class CreateProductionOrderCommandHandler : IRequestHandler<CreateProductionOrderCommand, Guid>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateProductionOrderCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateProductionOrderCommand request, CancellationToken cancellationToken)
    {
        var salesOrderItem = await _db.SalesOrderItems.FirstOrDefaultAsync(x => x.Id == request.SalesOrderItemId, cancellationToken);
        if (salesOrderItem is null)
            throw new NotFoundException("Sales order item not found");

        var maxNumber = await _db.ProductionOrders
            .OrderByDescending(x => x.ProductionOrderNumber)
            .Select(x => x.ProductionOrderNumber)
            .FirstOrDefaultAsync(cancellationToken);
        int nextNumber = 1;
        if (!string.IsNullOrWhiteSpace(maxNumber) && maxNumber.StartsWith("PO-") && int.TryParse(maxNumber[3..], out var parsed))
            nextNumber = parsed + 1;
        var orderNumber = $"PO-{nextNumber:D4}";

        var entity = new ProductionOrderEntity(
            orderNumber,
            request.SalesOrderItemId,
            request.DepartmentId,
            request.QtyPlanned,
            _currentUser.UserId
        );

        _db.ProductionOrders.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
