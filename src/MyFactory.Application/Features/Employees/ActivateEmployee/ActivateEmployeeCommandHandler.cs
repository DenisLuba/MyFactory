using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Employees.ActivateEmployee;

public sealed class ActivateEmployeeCommandHandler
    : IRequestHandler<ActivateEmployeeCommand>
{
    private readonly IApplicationDbContext _db;

    public ActivateEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        ActivateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _db.Employees
            .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Employee not found");

        if (employee.IsActive)
            return;

        if (employee.FiredAt.HasValue && request.HiredAt < employee.FiredAt.Value)
            throw new DomainException("Rehire date cannot be earlier than fired date.");

        employee.Rehire(request.HiredAt);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
