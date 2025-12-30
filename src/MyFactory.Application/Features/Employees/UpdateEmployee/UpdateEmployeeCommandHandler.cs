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

        var positionExists = await _db.Positions
            .AnyAsync(x => x.Id == request.PositionId, cancellationToken);

        if (!positionExists)
            throw new NotFoundException("Position not found");

        employee.Update(
            request.FullName,
            request.PositionId,
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
