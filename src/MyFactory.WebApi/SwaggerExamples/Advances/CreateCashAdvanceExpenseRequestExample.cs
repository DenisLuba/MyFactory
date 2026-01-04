using MyFactory.WebApi.Contracts.Advances;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Advances;

public sealed class CreateCashAdvanceExpenseRequestExample : IExamplesProvider<CreateCashAdvanceExpenseRequest>
{
    public CreateCashAdvanceExpenseRequest GetExamples() => new(
        ExpenseDate: new DateOnly(2025, 3, 22),
        Amount: 3200m,
        Description: "“кань и фурнитура");
}
