using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.AddProductionStageEmployee;

public sealed class AddProductionStageEmployeeCommandHandler : IRequestHandler<AddProductionStageEmployeeCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public AddProductionStageEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(AddProductionStageEmployeeCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.ProductionOrders
            .Include(x => x.ProductionOrderDepartmentEmployees)
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found.");

        var employee = await _db.Employees
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Employee not found.");

        var assignment = order.AddStageAssignment(
            stage: request.Stage,
            employee: employee,
            workDate: request.Date,
            assignedQty: request.AssignedQty,
            completedQty: request.CompletedQty);

        _db.ProductionOrderDepartmentEmployees.Add(assignment);

        await _db.SaveChangesAsync(cancellationToken);
        return assignment.Id;
    }
}
