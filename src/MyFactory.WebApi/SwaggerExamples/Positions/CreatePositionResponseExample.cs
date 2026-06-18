using MyFactory.WebApi.Contracts.Positions;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Positions;

public sealed class CreatePositionResponseExample : IExamplesProvider<CreatePositionResponse>
{
    public CreatePositionResponse GetExamples() => new(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"));
}
