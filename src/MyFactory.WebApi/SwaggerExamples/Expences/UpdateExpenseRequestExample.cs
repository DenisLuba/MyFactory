using MyFactory.WebApi.Contracts.Expences;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Expences;

public sealed class UpdateExpenseRequestExample : IExamplesProvider<UpdateExpenseRequest>
{
    public UpdateExpenseRequest GetExamples() => new(
        ExpenseDate: new DateOnly(2025, 3, 12),
        ExpenseTypeId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
        Amount: 18000m,
        Description: "Обновлённая сумма по договору");
}
