using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.PayrollRules.DeletePayrollRule;

public sealed class DeletePayrollRuleCommandHandler : IRequestHandler<DeletePayrollRuleCommand>
{
    private readonly IApplicationDbContext _db;

    public DeletePayrollRuleCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(DeletePayrollRuleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.PayrollRules
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Payroll rule not found");

        _db.PayrollRules.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
