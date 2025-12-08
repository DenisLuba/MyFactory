using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Domain.Entities.FinishedGoods;

namespace MyFactory.Application.Features.FinishedGoods.Commands.ReceiveFinishedGoods;

public sealed class ReceiveFinishedGoodsCommandHandler : IRequestHandler<ReceiveFinishedGoodsCommand, FinishedGoodsInventoryDto>
{
    private readonly IApplicationDbContext _context;

    public ReceiveFinishedGoodsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FinishedGoodsInventoryDto> Handle(ReceiveFinishedGoodsCommand request, CancellationToken cancellationToken)
    {
        var specification = await _context.Specifications
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.SpecificationId, cancellationToken);

        if (specification is null)
        {
            throw new InvalidOperationException("Specification not found.");
        }

        var warehouse = await _context.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.WarehouseId, cancellationToken);

        if (warehouse is null)
        {
            throw new InvalidOperationException("Warehouse not found.");
        }

        var inventory = await _context.FinishedGoodsInventories
            .FirstOrDefaultAsync(
                entity => entity.SpecificationId == request.SpecificationId && entity.WarehouseId == request.WarehouseId,
                cancellationToken);

        if (inventory is null)
        {
            inventory = new FinishedGoodsInventory(request.SpecificationId, request.WarehouseId);
            await _context.FinishedGoodsInventories.AddAsync(inventory, cancellationToken);
        }

        inventory.Receive(request.Quantity, request.UnitCost, request.ReceivedAt);

        await _context.SaveChangesAsync(cancellationToken);

        return FinishedGoodsInventoryDto.FromEntity(inventory, specification.Name, warehouse.Name);
    }
}
