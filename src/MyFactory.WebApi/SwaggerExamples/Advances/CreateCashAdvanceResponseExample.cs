using MyFactory.WebApi.Contracts.Advances;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Advances;

public sealed class CreateCashAdvanceResponseExample : IExamplesProvider<CreateCashAdvanceResponse>
{
    public CreateCashAdvanceResponse GetExamples() => new(Guid.Parse("bbbbbbbb-bb01-4a0a-b001-bbbbbbbb0001"));
}
