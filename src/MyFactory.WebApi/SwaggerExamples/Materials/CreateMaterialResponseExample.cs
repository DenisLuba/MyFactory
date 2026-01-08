using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public sealed class CreateMaterialResponseExample : IExamplesProvider<CreateMaterialResponse>
{
    public CreateMaterialResponse GetExamples() => new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0001"));
}
