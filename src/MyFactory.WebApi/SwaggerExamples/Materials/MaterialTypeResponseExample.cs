using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Materials;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class MaterialTypeResponseExample : IExamplesProvider<MaterialTypeResponse>
{
    public MaterialTypeResponse GetExamples() => new MaterialTypeResponse(
        Id: Guid.Parse("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        Name: "“кань"
    );
}