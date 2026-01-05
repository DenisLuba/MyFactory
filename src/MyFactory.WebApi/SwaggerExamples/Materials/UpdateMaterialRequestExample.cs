using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public sealed class UpdateMaterialRequestExample : IExamplesProvider<UpdateMaterialRequest>
{
    public UpdateMaterialRequest GetExamples() => new(
        Name: "Ситец отбеленный",
        MaterialTypeId: Guid.Parse("11111111-2222-3333-4444-555555555555"),
        UnitId: Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
        Color: "Белый");
}
