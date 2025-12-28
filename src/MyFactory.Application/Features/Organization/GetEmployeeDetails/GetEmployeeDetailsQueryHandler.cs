using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Organization;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Application.Features.GetEmployeeDetails;

public sealed class GetEmployeeDetailsQueryHandler
    : IRequestHandler<GetEmployeeDetailsQuery, EmployeeDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetEmployeeDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<EmployeeDetailsDto> Handle(
        GetEmployeeDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await _db.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Employee not found");

        var position = await _db.Positions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == employee.PositionId, cancellationToken)
            ?? throw new NotFoundException("Position not found");

        var department = await _db.Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == position.DepartmentId, cancellationToken)
            ?? throw new NotFoundException("Department not found");

        var contacts =
            await (
                from cl in _db.ContactLinks.AsNoTracking()
                join c in _db.Contacts.AsNoTracking()
                    on cl.ContactId equals c.Id
                where cl.OwnerId == employee.Id
                   && cl.OwnerType == ContactOwnerType.Employee
                orderby c.IsPrimary descending
                select new ContactDto
                {
                    Type = c.ContactType.ToString(),
                    Value = c.Value
                }
            ).ToListAsync(cancellationToken);

        return new EmployeeDetailsDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Grade = employee.Grade,
            RatePerNormHour = employee.RatePerNormHour,
            PremiumPercent = employee.PremiumPercent,
            HiredAt = DateOnly.FromDateTime(employee.HiredAt),
            FiredAt = employee.FiredAt.HasValue
                ? DateOnly.FromDateTime(employee.FiredAt.Value)
                : null,
            IsActive = employee.IsActive,
            Position = new PositionDto
            {
                Id = position.Id,
                Name = position.Name,
                DepartmentName = department.Name
            },
            Contacts = contacts
        };
    }
}

