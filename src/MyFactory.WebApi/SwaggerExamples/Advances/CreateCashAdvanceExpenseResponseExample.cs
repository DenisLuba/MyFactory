using MyFactory.WebApi.Contracts.Advances;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Advances;

public sealed class CreateCashAdvanceExpenseResponseExample : IExamplesProvider<CreateCashAdvanceExpenseResponse>
{
    public CreateCashAdvanceExpenseResponse GetExamples() => new(Guid.Parse("cccccccc-cc01-4a0a-b001-cccccccc0001"));
}
