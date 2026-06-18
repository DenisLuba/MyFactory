using MyFactory.WebApi.Contracts.MaterialTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTypes;

public sealed class CreateMaterialTypeRequestExample : IExamplesProvider<CreateMaterialTypeRequest>
{
    public CreateMaterialTypeRequest GetExamples() => new(
        Name: "Ткань",
        Description: "Категория тканей и полотен");
}
