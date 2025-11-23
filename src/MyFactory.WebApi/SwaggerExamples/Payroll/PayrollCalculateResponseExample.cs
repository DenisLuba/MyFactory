using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Payroll;

namespace MyFactory.WebApi.SwaggerExamples.Payroll;

public class PayrollCalculateResponseExample : IExamplesProvider<PayrollCalculateResponse>
{
    public PayrollCalculateResponse GetExamples() =>
        new(
            Status: PayrollCalculatingStatus.CalculationStarted,
            From: new DateTime(2025, 11, 1),
            To: new DateTime(2025, 11, 30)
        );
}


