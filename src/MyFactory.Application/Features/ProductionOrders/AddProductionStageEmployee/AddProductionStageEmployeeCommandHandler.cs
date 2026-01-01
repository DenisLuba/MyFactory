using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.AddProductionStageEmployee;

public sealed class AddProductionStageEmployeeCommandHandler : IRequestHandler<AddProductionStageEmployeeCommand>
{
    private readonly IApplicationDbContext _db;

    public AddProductionStageEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(AddProductionStageEmployeeCommand request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");
        if (po.Status != request.Stage)
            throw new DomainException("Cannot add operation: stage does not match order status.");

        //var stageQty = request.QtyCompleted;
        //var stage = MapStage(request.Stage);
        //var workDate = request.Date;

        if (request.Stage == ProductionOrderStatus.Cutting)
        {
            if (po.QtyCut + request.QtyCompleted > po.QtyPlanned)
                throw new DomainException("Cannot cut more than planned.");
            var op = new CuttingOperationEntity(request.ProductionOrderId, request.EmployeeId, request.QtyPlanned, request.QtyCompleted, request.Date);
            _db.CuttingOperations.Add(op);
            po.AddCut(request.QtyCompleted);
        }
        else if (request.Stage == ProductionOrderStatus.Sewing)
        {
            if (po.QtySewn + request.QtyCompleted > po.QtyCut)
                throw new DomainException("Cannot sew more than cut.");
            var op = new SewingOperationEntity(request.ProductionOrderId, request.EmployeeId, request.QtyPlanned, request.QtyCompleted, request.HoursWorked!.Value, request.Date);
            _db.SewingOperations.Add(op);
            po.AddSewn(request.QtyCompleted);
        }
        else if (request.Stage == ProductionOrderStatus.Packaging)
        {
            if (po.QtyPacked + request.QtyCompleted > po.QtySewn)
                throw new DomainException("Cannot pack more than sewn.");
            var op = new PackagingOperationEntity(request.ProductionOrderId, request.EmployeeId, request.QtyPlanned, request.QtyCompleted, request.Date);
            _db.PackagingOperations.Add(op);
            po.AddPacked(request.QtyCompleted);
        }
        else
        {
            throw new DomainException("Invalid stage for operation.");
        }

        //var departmentId = po.DepartmentId;
        //var podEmployee = await _db.ProductionOrderDepartmentEmployees
        //    .FirstOrDefaultAsync(x =>
        //        x.ProductionOrderId == request.ProductionOrderId &&
        //        x.DepartmentId == departmentId &&
        //        x.EmployeeId == request.EmployeeId &&
        //        x.Stage == stage &&
        //        x.WorkDate == workDate,
        //        cancellationToken);

        //if (podEmployee is null)
        //{
        //    podEmployee = new ProductionOrderDepartmentEmployeeEntity(
        //        request.ProductionOrderId,
        //        request.EmployeeId,
        //        departmentId,
        //        stage,
        //        workDate,
        //        request.QtyPlanned);
        //    podEmployee.RegisterCompletion(stageQty);
        //    _db.ProductionOrderDepartmentEmployees.Add(podEmployee);
        //}
        //else
        //{
        //    var newAssigned = podEmployee.QtyAssigned + request.QtyPlanned;
        //    var newCompleted = podEmployee.QtyCompleted + stageQty;
        //    podEmployee.UpdateAssignment(newAssigned);
        //    podEmployee.RegisterCompletion(newCompleted);
        //}

        await _db.SaveChangesAsync(cancellationToken);
    }

    //private static ProductionStage MapStage(ProductionOrderStatus status) => status switch
    //{
    //    ProductionOrderStatus.Cutting => ProductionStage.Cutting,
    //    ProductionOrderStatus.Sewing => ProductionStage.Sewing,
    //    ProductionOrderStatus.Packaging => ProductionStage.Packaging,
    //    _ => throw new DomainException("Invalid stage for production order department employee")
    //};
}
