using MyFactory.WebApi.Contracts.Advances;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Advances;

public sealed class CreateCashAdvanceRequestExample : IExamplesProvider<CreateCashAdvanceRequest>
{
    public CreateCashAdvanceRequest GetExamples() => new(
        EmployeeId: Guid.Parse("aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0001"),
        IssueDate: new DateOnly(2025, 3, 15),
        Amount: 15000m,
        Description: "Закупка материалов");
}
