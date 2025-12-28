using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
namespace MyFactory.Application.Features.DeactivateEmployee;

public sealed class DeactivateEmployeeCommandHandler
    : IRequestHandler<DeactivateEmployeeCommand>
{
    private readonly IApplicationDbContext _db;

    public DeactivateEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        DeactivateEmployeeCommand request,
        CancellationToken cancellationToken)
    {

        var employee = await _db.Employees
            .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Employee not found");

        if (!employee.IsActive)
            return;
        if (request.FiredAt < employee.HiredAt)
            throw new ArgumentException("Fired date cannot be earlier than hired date");

        employee.Fire(request.FiredAt);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
