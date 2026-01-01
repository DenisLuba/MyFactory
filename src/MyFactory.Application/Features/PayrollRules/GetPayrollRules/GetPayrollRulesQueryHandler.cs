using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.PayrollRules;

namespace MyFactory.Application.Features.PayrollRules.GetPayrollRules;

public sealed class GetPayrollRulesQueryHandler
    : IRequestHandler<GetPayrollRulesQuery, IReadOnlyList<PayrollRuleDto>>
{
    private readonly IApplicationDbContext _db;

    public GetPayrollRulesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<PayrollRuleDto>> Handle(GetPayrollRulesQuery request, CancellationToken cancellationToken)
    {
        return await _db.PayrollRules
            .AsNoTracking()
            .OrderByDescending(x => x.EffectiveFrom)
            .Select(x => new PayrollRuleDto(x.Id, x.EffectiveFrom, x.PremiumPercent, x.Description))
            .ToListAsync(cancellationToken);
    }
}
