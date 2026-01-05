using MyFactory.WebApi.Contracts.PayrollRules;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.PayrollRules;

public sealed class UpdatePayrollRuleRequestExample : IExamplesProvider<UpdatePayrollRuleRequest>
{
    public UpdatePayrollRuleRequest GetExamples() => new(
        EffectiveFrom: new DateOnly(2025, 5, 1),
        PremiumPercent: 0.28m,
        Description: "Корректировка процента");
}
