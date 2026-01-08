using MyFactory.WebApi.Contracts.MaterialTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTypes;

public sealed class MaterialTypeDetailsResponseExample : IExamplesProvider<MaterialTypeResponse>
{
    public MaterialTypeResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Name: "Ткань",
        Description: "Категория тканей");
}
