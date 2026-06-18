using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.RegisterSewingOperation;

public sealed class RegisterSewingOperationCommandHandler
    : IRequestHandler<RegisterSewingOperationCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public RegisterSewingOperationCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(RegisterSewingOperationCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.ProductionOrders
            .Include(x => x.ProductionOrderDepartmentEmployees)
            .Include(x => x.SewingOperations)
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found.");

        var assignment = order.ProductionOrderDepartmentEmployees
            .FirstOrDefault(x => x.Id == request.AssignmentId)
            ?? throw new NotFoundException("Sewing assignment not found.");

        if (assignment.Stage != ProductionStage.Sewing)
            throw new DomainApplicationException("Specified assignment is not a sewing assignment.");

        if (assignment.EmployeeId != request.EmployeeId)
            throw new DomainApplicationException("Sewing operation employee must match assignment employee.");

        var planPerHour = await (
            from salesOrderItem in _db.SalesOrderItems.AsNoTracking()
            join product in _db.Products.AsNoTracking()
                on salesOrderItem.ProductId equals product.Id
            where salesOrderItem.Id == order.SalesOrderItemId
            select product.PlanPerHour
        ).FirstOrDefaultAsync(cancellationToken);

        if (!planPerHour.HasValue || planPerHour.Value <= 0)
            throw new DomainApplicationException("Product plan per hour must be configured to register sewing operations.");

        var qtyPlanned = planPerHour.Value * request.HoursWorked;

        var operation = order.RegisterSewingOperation(
            assignmentId: request.AssignmentId,
            employeeId: request.EmployeeId,
            qtyPlanned: qtyPlanned,
            qtySewn: request.QtySewn,
            hoursWorked: request.HoursWorked,
            operationDate: request.OperationDate);

        _db.SewingOperations.Add(operation);

        await _db.SaveChangesAsync(cancellationToken);
        return operation.Id;
    }
}
