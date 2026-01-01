using MediatR;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.PayrollRules.CreatePayrollRule;

public sealed class CreatePayrollRuleCommandHandler
    : IRequestHandler<CreatePayrollRuleCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreatePayrollRuleCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreatePayrollRuleCommand request, CancellationToken cancellationToken)
    {
        // Допускаем несколько правил на одну дату (для разных продуктов/контекстов)
        var entity = new PayrollRuleEntity(request.EffectiveFrom, request.PremiumPercent, request.Description);

        _db.PayrollRules.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
