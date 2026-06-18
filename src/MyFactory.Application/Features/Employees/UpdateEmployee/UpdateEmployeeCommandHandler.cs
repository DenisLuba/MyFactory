using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Employees.UpdateEmployee;

public sealed class UpdateEmployeeCommandHandler
    : IRequestHandler<UpdateEmployeeCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _db.Employees
            .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Employee not found");

        var position = await _db.Positions
            .Include(p => p.DepartmentPositions)
            .FirstOrDefaultAsync(x => x.Id == request.PositionId, cancellationToken)
            ?? throw new NotFoundException("Position not found");

        var departmentExists = await _db.Departments
            .AnyAsync(x => x.Id == request.DepartmentId, cancellationToken);

        if (!departmentExists)
            throw new NotFoundException("Department not found");

        var positionInDepartment = position.DepartmentPositions.Any(dp => dp.DepartmentId == request.DepartmentId);
        if (!positionInDepartment)
            throw new DomainApplicationException("Position is not linked to the specified department.");

        employee.Update(
            request.FullName,
            request.PositionId,
            request.DepartmentId,
            request.Grade,
            request.RatePerNormHour,
            request.PremiumPercent,
            request.HiredAt);

        if (!request.IsActive && employee.IsActive)
        {
            employee.Fire(DateTime.UtcNow);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
