using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Domain.Entities.FinishedGoods;

namespace MyFactory.Application.Features.FinishedGoods.Commands.MoveFinishedGoods;

public sealed class MoveFinishedGoodsCommandHandler : IRequestHandler<MoveFinishedGoodsCommand, FinishedGoodsMovementDto>
{
    private readonly IApplicationDbContext _context;

    public MoveFinishedGoodsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FinishedGoodsMovementDto> Handle(MoveFinishedGoodsCommand request, CancellationToken cancellationToken)
    {
        var specification = await _context.Specifications
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.SpecificationId, cancellationToken)
            ?? throw new InvalidOperationException("Specification not found.");

        var fromWarehouse = await _context.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.FromWarehouseId, cancellationToken)
            ?? throw new InvalidOperationException("Source warehouse not found.");

        var toWarehouse = await _context.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.ToWarehouseId, cancellationToken)
            ?? throw new InvalidOperationException("Destination warehouse not found.");

        var fromInventory = await _context.FinishedGoodsInventories
            .FirstOrDefaultAsync(
                entity => entity.SpecificationId == request.SpecificationId && entity.WarehouseId == request.FromWarehouseId,
                cancellationToken)
            ?? throw new InvalidOperationException("Source inventory not found.");

        var unitCost = fromInventory.UnitCost;
        var sourceAvailableQuantity = fromInventory.Quantity;
        fromInventory.Issue(request.Quantity, request.MovedAt);

        var toInventory = await _context.FinishedGoodsInventories
            .FirstOrDefaultAsync(
                entity => entity.SpecificationId == request.SpecificationId && entity.WarehouseId == request.ToWarehouseId,
                cancellationToken);

        if (toInventory is null)
        {
            toInventory = new FinishedGoodsInventory(request.SpecificationId, request.ToWarehouseId);
            await _context.FinishedGoodsInventories.AddAsync(toInventory, cancellationToken);
        }

        toInventory.Receive(request.Quantity, unitCost, request.MovedAt);

        var movement = FinishedGoodsMovement.CreateTransfer(
            request.SpecificationId,
            request.FromWarehouseId,
            request.ToWarehouseId,
            request.Quantity,
            request.MovedAt,
            fromInventory.Id,
            sourceAvailableQuantity);

        await _context.FinishedGoodsMovements.AddAsync(movement, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return FinishedGoodsMovementDto.FromEntity(
            movement,
            specification.Name,
            fromWarehouse.Name,
            toWarehouse.Name);
    }
}
