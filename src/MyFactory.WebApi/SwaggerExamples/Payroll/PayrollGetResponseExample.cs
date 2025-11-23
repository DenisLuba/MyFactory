using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Payroll;

namespace MyFactory.WebApi.SwaggerExamples.Payroll;

public class PayrollGetResponseExample : IExamplesProvider<PayrollGetResponse>
{
    public PayrollGetResponse GetExamples() =>
        new(
            EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Period: "11.2025",
            Accrued: 32500m,
            Paid: 15000m,
            Outstanding: 17500m
        );
}


