using MyFactory.WebApi.Contracts.PayrollRules;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.PayrollRules;

public sealed class CreatePayrollRuleResponseExample : IExamplesProvider<CreatePayrollRuleResponse>
{
    public CreatePayrollRuleResponse GetExamples() => new(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"));
}
