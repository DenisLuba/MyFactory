using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Application.Features.Employees.CreateEmployee;

public sealed class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
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

        var employee = new EmployeeEntity(
            request.FullName,
            request.PositionId,
            request.DepartmentId,
            request.Grade,
            request.RatePerNormHour,
            request.PremiumPercent,
            request.HiredAt);

        if (!request.IsActive)
        {
            employee.Fire(request.HiredAt);
        }

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync(cancellationToken);

        return employee.Id;
    }
}
