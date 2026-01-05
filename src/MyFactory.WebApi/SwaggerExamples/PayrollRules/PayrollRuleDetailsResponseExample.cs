using MyFactory.WebApi.Contracts.PayrollRules;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.PayrollRules;

public sealed class PayrollRuleDetailsResponseExample : IExamplesProvider<PayrollRuleResponse>
{
    public PayrollRuleResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        EffectiveFrom: new DateOnly(2025, 1, 1),
        PremiumPercent: 0.2m,
        Description: "Базовое правило");
}
