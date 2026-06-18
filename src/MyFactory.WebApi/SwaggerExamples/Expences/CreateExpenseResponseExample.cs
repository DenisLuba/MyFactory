using MyFactory.WebApi.Contracts.Expences;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Expences;

public sealed class CreateExpenseResponseExample : IExamplesProvider<CreateExpenseResponse>
{
    public CreateExpenseResponse GetExamples() => new(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"));
}
