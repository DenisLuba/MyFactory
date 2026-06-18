using MyFactory.WebApi.Contracts.Advances;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Advances;

public sealed class CreateCashAdvanceReturnResponseExample : IExamplesProvider<CreateCashAdvanceReturnResponse>
{
    public CreateCashAdvanceReturnResponse GetExamples() => new(Guid.Parse("dddddddd-dd01-4a0a-b001-dddddddd0001"));
}
