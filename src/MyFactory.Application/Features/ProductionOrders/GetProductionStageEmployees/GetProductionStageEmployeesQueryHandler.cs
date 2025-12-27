using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductionOrders;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionStageEmployees;

public sealed class GetProductionStageEmployeesQueryHandler : IRequestHandler<GetProductionStageEmployeesQuery, IReadOnlyList<ProductionStageEmployeeDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductionStageEmployeesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductionStageEmployeeDto>> Handle(GetProductionStageEmployeesQuery request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");

        if (request.Stage == ProductionOrderStatus.Cutting)
        {
            var query =
                from op in _db.CuttingOperations.AsNoTracking()
                join emp in _db.Employees.AsNoTracking() on op.EmployeeId equals emp.Id
                where op.ProductionOrderId == request.ProductionOrderId
                group op by new { op.EmployeeId, emp.FullName } into g
                select new ProductionStageEmployeeDto
                {
                    EmployeeId = g.Key.EmployeeId,
                    EmployeeName = g.Key.FullName,
                    PlanPerHour = null,
                    AssignedQty = g.Sum(x => x.QtyPlanned),
                    CompletedQty = g.Sum(x => x.QtyCut)
                };
            return await query.ToListAsync(cancellationToken);
        }
        if (request.Stage == ProductionOrderStatus.Sewing)
        {
            var productId = await _db.SalesOrderItems.AsNoTracking()
                .Where(x => x.Id == po.SalesOrderItemId)
                .Select(x => x.ProductId)
                .FirstOrDefaultAsync(cancellationToken);
            var planPerHour = await _db.Products.AsNoTracking()
                .Where(x => x.Id == productId)
                .Select(x => x.PlanPerHour)
                .FirstOrDefaultAsync(cancellationToken);
            var query =
                from op in _db.SewingOperations.AsNoTracking()
                join emp in _db.Employees.AsNoTracking() on op.EmployeeId equals emp.Id
                where op.ProductionOrderId == request.ProductionOrderId
                group op by new { op.EmployeeId, emp.FullName } into g
                select new ProductionStageEmployeeDto
                {
                    EmployeeId = g.Key.EmployeeId,
                    EmployeeName = g.Key.FullName,
                    PlanPerHour = planPerHour,
                    AssignedQty = g.Sum(x => x.QtyPlanned),
                    CompletedQty = g.Sum(x => x.QtySewn)
                };
            return await query.ToListAsync(cancellationToken);
        }
        if (request.Stage == ProductionOrderStatus.Packaging)
        {
            var query =
                from op in _db.PackagingOperations.AsNoTracking()
                join emp in _db.Employees.AsNoTracking() on op.EmployeeId equals emp.Id
                where op.ProductionOrderId == request.ProductionOrderId
                group op by new { op.EmployeeId, emp.FullName } into g
                select new ProductionStageEmployeeDto
                {
                    EmployeeId = g.Key.EmployeeId,
                    EmployeeName = g.Key.FullName,
                    PlanPerHour = null,
                    AssignedQty = g.Sum(x => x.QtyPlanned),
                    CompletedQty = g.Sum(x => x.QtyPacked)
                };
            return await query.ToListAsync(cancellationToken);
        }
        return new List<ProductionStageEmployeeDto>();
    }
}
