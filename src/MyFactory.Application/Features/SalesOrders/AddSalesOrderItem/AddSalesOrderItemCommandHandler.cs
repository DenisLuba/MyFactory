using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.SalesOrders.AddSalesOrderItem;

public sealed class AddSalesOrderItemCommandHandler : IRequestHandler<AddSalesOrderItemCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public AddSalesOrderItemCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(AddSalesOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.SalesOrders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException("Sales order not found");
        if (order.Status != SalesOrderStatus.New)
            throw new DomainApplicationException("Only new orders can be modified.");
        // Check if product exists
        var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
            ?? throw new NotFoundException("Product not found");
        var item = new SalesOrderItemEntity(request.OrderId, request.ProductId, request.Qty);
        _db.SalesOrderItems.Add(item);
        await _db.SaveChangesAsync(cancellationToken);
        return item.Id;
    }
}
