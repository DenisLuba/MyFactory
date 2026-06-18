using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public sealed class CreatePayrollPaymentRequestExample : IExamplesProvider<CreatePayrollPaymentRequest>
{
    public CreatePayrollPaymentRequest GetExamples() => new(
        EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        PaymentDate: new DateOnly(2025, 3, 25),
        Amount: 15000m);
}
