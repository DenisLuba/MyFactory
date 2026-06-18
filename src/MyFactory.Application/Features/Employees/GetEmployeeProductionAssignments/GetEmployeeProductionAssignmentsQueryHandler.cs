using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Employees;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.Employees.GetEmployeeProductionAssignments;

public sealed class GetEmployeeProductionAssignmentsQueryHandler
    : IRequestHandler<GetEmployeeProductionAssignmentsQuery, IReadOnlyList<EmployeeProductionAssignmentDto>>
{
    private readonly IApplicationDbContext _db;

    public GetEmployeeProductionAssignmentsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<EmployeeProductionAssignmentDto>> Handle(
        GetEmployeeProductionAssignmentsQuery request,
        CancellationToken cancellationToken)
    {
        var assignments =
            from assignment in _db.ProductionOrderDepartmentEmployees.AsNoTracking()
            join po in _db.ProductionOrders.AsNoTracking()
                on assignment.ProductionOrderId equals po.Id
            where assignment.EmployeeId == request.EmployeeId
            select new EmployeeProductionAssignmentDto
            {
                ProductionOrderId = po.Id,
                ProductionOrderNumber = po.ProductionOrderNumber,
                Stage = assignment.Stage == ProductionStage.Cutting
                    ? ProductionOrderStatus.Cutting
                    : assignment.Stage == ProductionStage.Sewing
                        ? ProductionOrderStatus.Sewing
                        : ProductionOrderStatus.Packaging,
                QtyAssigned = assignment.AssignedQty,
                QtyCompleted = assignment.CompletedQty
            };

        return await assignments
            .OrderBy(x => x.ProductionOrderNumber)
            .ToListAsync(cancellationToken);
    }
}
