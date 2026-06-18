using MyFactory.WebApi.Contracts.Advances;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Advances;

public sealed class AddCashAdvanceAmountRequestExample : IExamplesProvider<AddCashAdvanceAmountRequest>
{
    public AddCashAdvanceAmountRequest GetExamples() => new(
        IssueDate: new DateOnly(2025, 3, 20),
        Amount: 5000m);
}
