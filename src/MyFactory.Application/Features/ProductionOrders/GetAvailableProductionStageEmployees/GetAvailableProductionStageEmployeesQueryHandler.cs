using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Employees;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.GetAvailableProductionStageEmployees;

public sealed class GetAvailableProductionStageEmployeesQueryHandler
    : IRequestHandler<GetAvailableProductionStageEmployeesQuery, IReadOnlyList<EmployeeListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetAvailableProductionStageEmployeesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<EmployeeListItemDto>> Handle(
        GetAvailableProductionStageEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var departmentId = await _db.ProductionOrders
            .AsNoTracking()
            .Where(x => x.Id == request.ProductionOrderId)
            .Select(x => (Guid?)x.DepartmentId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Production order not found.");

        var query =
            from employee in _db.Employees.AsNoTracking()
            join position in _db.Positions.AsNoTracking() on employee.PositionId equals position.Id
            join department in _db.Departments.AsNoTracking() on employee.DepartmentId equals department.Id
            where employee.DepartmentId == departmentId
                  && employee.IsActive
                  && (
                      (request.Stage == ProductionStage.Cutting && position.CanCut) ||
                      (request.Stage == ProductionStage.Sewing && position.CanSew) ||
                      (request.Stage == ProductionStage.Packaging && position.CanPackage)
                  )
            orderby employee.FullName
            select new EmployeeListItemDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                DepartmentName = department.Name,
                PositionName = position.Name,
                IsActive = employee.IsActive
            };

        return await query.ToListAsync(cancellationToken);
    }
}
