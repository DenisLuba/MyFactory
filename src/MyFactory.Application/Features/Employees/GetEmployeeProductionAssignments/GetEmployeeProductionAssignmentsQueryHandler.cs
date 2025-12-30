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
        var cutting =
            from op in _db.CuttingOperations.AsNoTracking()
            join po in _db.ProductionOrders.AsNoTracking()
                on op.ProductionOrderId equals po.Id
            where op.EmployeeId == request.EmployeeId
            select new EmployeeProductionAssignmentDto
            {
                ProductionOrderId = po.Id,
                ProductionOrderNumber = po.ProductionOrderNumber,
                Stage = ProductionOrderStatus.Cutting,
                QtyAssigned = op.QtyPlanned,
                QtyCompleted = op.QtyCut
            };

        var sewing =
            from op in _db.SewingOperations.AsNoTracking()
            join po in _db.ProductionOrders.AsNoTracking()
                on op.ProductionOrderId equals po.Id
            where op.EmployeeId == request.EmployeeId
            select new EmployeeProductionAssignmentDto
            {
                ProductionOrderId = po.Id,
                ProductionOrderNumber = po.ProductionOrderNumber,
                Stage = ProductionOrderStatus.Sewing,
                QtyAssigned = op.QtyPlanned,
                QtyCompleted = op.QtySewn
            };

        var packaging =
            from op in _db.PackagingOperations.AsNoTracking()
            join po in _db.ProductionOrders.AsNoTracking()
                on op.ProductionOrderId equals po.Id
            where op.EmployeeId == request.EmployeeId
            select new EmployeeProductionAssignmentDto
            {
                ProductionOrderId = po.Id,
                ProductionOrderNumber = po.ProductionOrderNumber,
                Stage = ProductionOrderStatus.Packaging,
                QtyAssigned = op.QtyPlanned,
                QtyCompleted = op.QtyPacked
            };

        return await cutting
            .Concat(sewing)
            .Concat(packaging)
            .OrderBy(x => x.ProductionOrderNumber)
            .ToListAsync(cancellationToken);
    }
}
