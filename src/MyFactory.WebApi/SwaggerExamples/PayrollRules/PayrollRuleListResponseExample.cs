using MyFactory.WebApi.Contracts.PayrollRules;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.PayrollRules;

public sealed class PayrollRuleListResponseExample : IExamplesProvider<IReadOnlyList<PayrollRuleResponse>>
{
    public IReadOnlyList<PayrollRuleResponse> GetExamples() => new List<PayrollRuleResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            EffectiveFrom: new DateOnly(2025, 1, 1),
            PremiumPercent: 0.2m,
            Description: "Базовое правило"),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            EffectiveFrom: new DateOnly(2025, 3, 15),
            PremiumPercent: 0.3m,
            Description: "Повышенный % для сезона")
    };
}
