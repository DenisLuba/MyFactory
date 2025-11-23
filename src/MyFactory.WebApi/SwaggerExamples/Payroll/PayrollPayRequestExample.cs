using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Payroll;

namespace MyFactory.WebApi.SwaggerExamples.Payroll;

public class PayrollPayRequestExample : IExamplesProvider<PayrollPayRequest>
{
    public PayrollPayRequest GetExamples() =>
        new(
            EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-bbbbbbbbbbbb"),
            Amount: 20000m,
            Date: new DateTime(2025, 11, 20)
        );
}


