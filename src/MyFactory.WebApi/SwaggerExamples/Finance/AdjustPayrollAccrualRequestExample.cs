using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public sealed class AdjustPayrollAccrualRequestExample : IExamplesProvider<AdjustPayrollAccrualRequest>
{
    public AdjustPayrollAccrualRequest GetExamples() => new(
        BaseAmount: 1500m,
        PremiumAmount: 300m,
        Reason: "Корректировка за переработку");
}
