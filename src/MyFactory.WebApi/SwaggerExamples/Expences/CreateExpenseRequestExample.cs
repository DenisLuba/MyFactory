using MyFactory.WebApi.Contracts.Expences;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Expences;

public sealed class CreateExpenseRequestExample : IExamplesProvider<CreateExpenseRequest>
{
    public CreateExpenseRequest GetExamples() => new(
        ExpenseTypeId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
        ExpenseDate: new DateOnly(2025, 3, 10),
        Amount: 15000m,
        Description: "Закупка расходников");
}
