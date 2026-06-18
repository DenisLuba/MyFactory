using MyFactory.WebApi.Contracts.ExpenceTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ExpenceTypes;

public sealed class CreateExpenseTypeResponseExample : IExamplesProvider<CreateExpenseTypeResponse>
{
    public CreateExpenseTypeResponse GetExamples() => new(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"));
}
