using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Application.Features.Advances.CreateCashAdvance;

public sealed class CreateCashAdvanceCommandHandler : IRequestHandler<CreateCashAdvanceCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateCashAdvanceCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateCashAdvanceCommand request, CancellationToken cancellationToken)
    {
        var employeeExists = await _db.Employees
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.EmployeeId, cancellationToken);

        if (!employeeExists)
            throw new DomainException("Employee not found.");

        var advance = new CashAdvanceEntity(request.EmployeeId, request.IssueDate, request.Amount);
        _db.CashAdvances.Add(advance);
        await _db.SaveChangesAsync(cancellationToken);

        return advance.Id;
    }
}
