using MyFactory.WebApi.Contracts.MaterialTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTypes;

public sealed class CreateMaterialTypeResponseExample : IExamplesProvider<CreateMaterialTypeResponse>
{
    public CreateMaterialTypeResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"));
}
