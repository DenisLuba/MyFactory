using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Warehouses.AddProductToWarehouse;

public sealed class AddProductToWarehouseCommandHandler
    : IRequestHandler<AddProductToWarehouseCommand>
{
    private readonly IApplicationDbContext _db;

    public AddProductToWarehouseCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(AddProductToWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await _db.Warehouses
            .FirstOrDefaultAsync(x => x.Id == request.WarehouseId, cancellationToken)
            ?? throw new NotFoundException("Warehouse not found");

        if (warehouse.Type != WarehouseType.FinishedGoods)
            throw new ValidationException("Can only add products to a finished goods warehouse");

        var productExists = await _db.Products
            .AnyAsync(x => x.Id == request.ProductId, cancellationToken);

        if (!productExists)
            throw new NotFoundException("Product not found");

        var stock = await _db.FinishedGoodsStocks
            .FirstOrDefaultAsync(x => x.WarehouseId == request.WarehouseId && x.ProductId == request.ProductId, cancellationToken);

        if (stock is null)
        {
            stock = new FinishedGoodsStockEntity(request.WarehouseId, request.ProductId, request.Qty);
            _db.FinishedGoodsStocks.Add(stock);
        }
        else
        {
            stock.AddQty(request.Qty);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
