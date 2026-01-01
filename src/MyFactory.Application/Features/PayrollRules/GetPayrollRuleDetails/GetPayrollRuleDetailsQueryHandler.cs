using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.PayrollRules;

namespace MyFactory.Application.Features.PayrollRules.GetPayrollRuleDetails;

public sealed class GetPayrollRuleDetailsQueryHandler
    : IRequestHandler<GetPayrollRuleDetailsQuery, PayrollRuleDto>
{
    private readonly IApplicationDbContext _db;

    public GetPayrollRuleDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PayrollRuleDto> Handle(GetPayrollRuleDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _db.PayrollRules
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Payroll rule not found");

        return new PayrollRuleDto(entity.Id, entity.EffectiveFrom, entity.PremiumPercent, entity.Description);
    }
}
