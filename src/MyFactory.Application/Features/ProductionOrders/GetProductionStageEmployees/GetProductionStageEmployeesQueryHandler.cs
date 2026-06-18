using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductionOrders;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionStageEmployees;

public sealed class GetProductionStageEmployeesQueryHandler
    : IRequestHandler<GetProductionStageEmployeesQuery, IReadOnlyList<ProductionStageAssignmentDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductionStageEmployeesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductionStageAssignmentDto>> Handle(
        GetProductionStageEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var productionOrder = await _db.ProductionOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found.");

        decimal? planPerHour = null;

        if (request.Stage == ProductionStage.Sewing)
        {
            var productId = await _db.SalesOrderItems
                .AsNoTracking()
                .Where(x => x.Id == productionOrder.SalesOrderItemId)
                .Select(x => x.ProductId)
                .FirstOrDefaultAsync(cancellationToken);

            if (productId != Guid.Empty)
            {
                planPerHour = await _db.Products
                    .AsNoTracking()
                    .Where(x => x.Id == productId)
                    .Select(x => x.PlanPerHour)
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }

        var assignmentsQuery =
            from assignment in _db.ProductionOrderDepartmentEmployees.AsNoTracking()
            join employee in _db.Employees.AsNoTracking()
                on assignment.EmployeeId equals employee.Id
            where assignment.ProductionOrderId == request.ProductionOrderId
                  && assignment.Stage == request.Stage
            orderby assignment.WorkDate, employee.FullName
            select new ProductionStageAssignmentDto
            {
                AssignmentId = assignment.Id,
                Stage = assignment.Stage,
                EmployeeId = assignment.EmployeeId,
                EmployeeName = employee.FullName,
                PlanPerHour = request.Stage == ProductionStage.Sewing ? planPerHour : null,
                AssignedQty = assignment.AssignedQty,
                CompletedQty = assignment.CompletedQty,
                WorkDate = assignment.WorkDate
            };

        return await assignmentsQuery.ToListAsync(cancellationToken);
    }
}
