using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Payroll;

namespace MyFactory.WebApi.SwaggerExamples.Payroll;

public class PayrollPayResponseExample : IExamplesProvider<PayrollPayResponse>
{
    public PayrollPayResponse GetExamples() =>
        new(Status: PayrollPaymentStatus.Paid);
}


