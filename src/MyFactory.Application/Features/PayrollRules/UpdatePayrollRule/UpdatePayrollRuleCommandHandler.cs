using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.PayrollRules.UpdatePayrollRule;

public sealed class UpdatePayrollRuleCommandHandler : IRequestHandler<UpdatePayrollRuleCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdatePayrollRuleCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdatePayrollRuleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.PayrollRules
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Payroll rule not found");

        entity.Update(request.EffectiveFrom, request.PremiumPercent, request.Description);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
