using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public sealed class CreateMaterialRequestExample : IExamplesProvider<CreateMaterialRequest>
{
    public CreateMaterialRequest GetExamples() => new(
        Name: "Ткань Ситец",
        MaterialTypeId: Guid.Parse("b5e0a3b5-1f6c-4a37-bfe6-6b4d64b74b4a"),
        UnitId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Color: "Белый");
}
