using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.RemoveProductionStageEmployee;

public sealed class RemoveProductionStageEmployeeCommandHandler : IRequestHandler<RemoveProductionStageEmployeeCommand>
{
    private readonly IApplicationDbContext _db;

    public RemoveProductionStageEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(RemoveProductionStageEmployeeCommand request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");
        if (po.Status != request.Stage)
            throw new DomainApplicationException("Stage does not match production order status.");

        //ProductionStage stage;
        //int stageQty;
        //int stagePlanned;
        //DateOnly operationDate;
        //Guid employeeId;

        if (request.Stage == ProductionOrderStatus.Cutting)
        {
            var op = await _db.CuttingOperations.FirstOrDefaultAsync(x => x.Id == request.OperationId, cancellationToken)
                ?? throw new NotFoundException("Cutting operation not found");
            _db.CuttingOperations.Remove(op);
            po.RemoveCut(op.QtyCut);

            //stage = ProductionStage.Cutting;
            //stageQty = op.QtyCut;
            //stagePlanned = op.QtyPlanned;
            //operationDate = op.OperationDate;
            //employeeId = op.EmployeeId;
        }
        else if (request.Stage == ProductionOrderStatus.Sewing)
        {
            var op = await _db.SewingOperations.FirstOrDefaultAsync(x => x.Id == request.OperationId, cancellationToken)
                ?? throw new NotFoundException("Sewing operation not found");
            _db.SewingOperations.Remove(op);
            po.RemoveSewn(op.QtySewn);

            //stage = ProductionStage.Sewing;
            //stageQty = op.QtySewn;
            //stagePlanned = op.QtyPlanned;
            //operationDate = op.OperationDate;
            //employeeId = op.EmployeeId;
        }
        else if (request.Stage == ProductionOrderStatus.Packaging)
        {
            var op = await _db.PackagingOperations.FirstOrDefaultAsync(x => x.Id == request.OperationId, cancellationToken)
                ?? throw new NotFoundException("Packaging operation not found");
            _db.PackagingOperations.Remove(op);
            po.RemovePacked(op.QtyPacked);

            //stage = ProductionStage.Packaging;
            //stageQty = op.QtyPacked;
            //stagePlanned = op.QtyPlanned;
            //operationDate = op.OperationDate;
            //employeeId = op.EmployeeId;
        }
        else
        {
            throw new NotFoundException("Invalid stage for operation removal.");
        }

        //var departmentId = po.DepartmentId;

        //var podEmployee = await _db.ProductionOrderDepartmentEmployees
        //    .FirstOrDefaultAsync(x =>
        //        x.ProductionOrderId == request.ProductionOrderId &&
        //        x.DepartmentId == departmentId &&
        //        x.EmployeeId == employeeId &&
        //        x.Stage == stage &&
        //        x.WorkDate == operationDate,
        //        cancellationToken);

        //if (podEmployee != null)
        //{
        //    var newAssigned = podEmployee.QtyAssigned - stagePlanned;
        //    var newCompleted = podEmployee.QtyCompleted - stageQty;

        //    if (newCompleted < 0)
        //        throw new DomainException("Cannot reduce completion below zero.");

        //    if (newAssigned <= 0)
        //    {
        //        _db.ProductionOrderDepartmentEmployees.Remove(podEmployee);
        //    }
        //    else
        //    {
        //        podEmployee.UpdateAssignment(newAssigned);
        //        podEmployee.RegisterCompletion(newCompleted);
        //    }
        //}

        await _db.SaveChangesAsync(cancellationToken);
    }
}
