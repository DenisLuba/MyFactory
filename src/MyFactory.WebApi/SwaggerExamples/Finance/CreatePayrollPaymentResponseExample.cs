using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public sealed class CreatePayrollPaymentResponseExample : IExamplesProvider<CreatePayrollPaymentResponse>
{
    public CreatePayrollPaymentResponse GetExamples() => new(Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0005"));
}
