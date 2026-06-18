using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.ProductionOrders.UpdateProductionStageEmployee;

public sealed class UpdateProductionStageEmployeeCommandHandler
    : IRequestHandler<UpdateProductionStageEmployeeCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateProductionStageEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateProductionStageEmployeeCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.ProductionOrders
            .Include(x => x.ProductionOrderDepartmentEmployees)
            .Include(x => x.SewingOperations)
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found.");

        var assignment = order.ProductionOrderDepartmentEmployees
            .FirstOrDefault(x => x.Id == request.AssignmentId)
            ?? throw new NotFoundException("Stage assignment not found.");

        if (assignment.Stage != request.Stage)
            throw new DomainApplicationException("Assignment stage does not match request stage.");

        var employee = await _db.Employees
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Employee not found.");

        order.UpdateStageAssignment(
            assignmentId: request.AssignmentId,
            employee: employee,
            workDate: request.Date,
            assignedQty: request.AssignedQty,
            completedQty: request.CompletedQty);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
