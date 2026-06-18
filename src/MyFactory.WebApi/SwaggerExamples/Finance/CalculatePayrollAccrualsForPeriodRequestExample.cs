using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public sealed class CalculatePayrollAccrualsForPeriodRequestExample : IExamplesProvider<CalculatePayrollAccrualsForPeriodRequest>
{
    public CalculatePayrollAccrualsForPeriodRequest GetExamples() => new(Year: 2025, Month: 3);
}
