using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.UpdateProductionStageEmployee;

public sealed class UpdateProductionStageEmployeeCommandHandler : IRequestHandler<UpdateProductionStageEmployeeCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateProductionStageEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateProductionStageEmployeeCommand request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");
        if (po.Status != request.Stage)
            throw new DomainException("Cannot update operation: stage does not match order status.");
        if (request.Stage == ProductionOrderStatus.Cutting)
        {
            var op = await _db.CuttingOperations.FirstOrDefaultAsync(x => x.Id == request.OperationId, cancellationToken)
                ?? throw new NotFoundException("Cutting operation not found");
            // Удаляем старую операцию
            _db.CuttingOperations.Remove(op);
            if (po.QtyCut - op.QtyCut + request.Qty > po.QtyPlanned)
                throw new DomainException("Cannot cut more than planned.");
            var newOp = new CuttingOperationEntity(request.ProductionOrderId, request.EmployeeId, request.QtyPlanned, request.Qty, request.Date);
            // Добавляем новую операцию
            _db.CuttingOperations.Add(newOp);
            po.RecalculateCutQty(
                await _db.CuttingOperations
                    .Where(x => x.ProductionOrderId == request.ProductionOrderId)
                    .SumAsync(x => x.QtyCut, cancellationToken)
                );
        }
        else if (request.Stage == ProductionOrderStatus.Sewing)
        {
            var op = await _db.SewingOperations.FirstOrDefaultAsync(x => x.Id == request.OperationId, cancellationToken)
                ?? throw new NotFoundException("Sewing operation not found");
            // Удаляем старую операцию
            _db.SewingOperations.Remove(op);
            if (po.QtySewn - op.QtySewn + request.Qty > po.QtyCut)
                throw new DomainException("Cannot sew more than cut.");
            var newOp = new SewingOperationEntity(request.ProductionOrderId, request.EmployeeId, request.QtyPlanned, request.Qty, request.HoursWorked!.Value, request.Date);
            // Добавляем новую операцию
            _db.SewingOperations.Add(newOp);
            po.RecalculateSewnQty(
                await _db.SewingOperations
                    .Where(x => x.ProductionOrderId == request.ProductionOrderId)
                    .SumAsync(x => x.QtySewn, cancellationToken)
                );
        }
        else if (request.Stage == ProductionOrderStatus.Packaging)
        {
            var op = await _db.PackagingOperations.FirstOrDefaultAsync(x => x.Id == request.OperationId, cancellationToken)
                ?? throw new NotFoundException("Packaging operation not found");
            // Удаляем старую операцию
            _db.PackagingOperations.Remove(op);
            if (po.QtyPacked - op.QtyPacked + request.Qty > po.QtySewn)
                throw new DomainException("Cannot pack more than sewn.");
            var newOp = new PackagingOperationEntity(request.ProductionOrderId, request.EmployeeId, request.QtyPlanned, request.Qty, request.Date);
            // Добавляем новую операцию
            _db.PackagingOperations.Add(newOp);
            po.RecalculatePackedQty(
                await _db.PackagingOperations
                    .Where(x => x.ProductionOrderId == request.ProductionOrderId)
                    .SumAsync(x => x.QtyPacked, cancellationToken)
                );
        }
        else
        {
            throw new DomainException("Invalid stage for operation.");
        }
        await _db.SaveChangesAsync(cancellationToken);
    }
}
