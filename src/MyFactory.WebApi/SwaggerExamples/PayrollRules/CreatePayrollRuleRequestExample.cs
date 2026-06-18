using MyFactory.WebApi.Contracts.PayrollRules;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.PayrollRules;

public sealed class CreatePayrollRuleRequestExample : IExamplesProvider<CreatePayrollRuleRequest>
{
    public CreatePayrollRuleRequest GetExamples() => new(
        Name:"Rule for April 2025",
        EffectiveFrom: new DateOnly(2025, 4, 1),
        PremiumPercent: 0.25m,
        Description: "Rule for April 2025");
}
