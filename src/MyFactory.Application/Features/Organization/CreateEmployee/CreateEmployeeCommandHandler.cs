using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Application.Features.CreateEmployee;

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
            .FirstOrDefaultAsync(x => x.Id == request.PositionId, cancellationToken)
            ?? throw new NotFoundException("Position not found");

        var employee = new EmployeeEntity(
            request.FullName,
            request.PositionId,
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
