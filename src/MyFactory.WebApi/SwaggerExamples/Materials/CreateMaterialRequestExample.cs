using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class CreateMaterialRequestExample : IExamplesProvider<CreateMaterialRequest>
{
    public CreateMaterialRequest GetExamples() =>
        new(
            Code : "MAT-001",
            Name : "Ткань Ситец",
            MaterialTypeId : Guid.Parse("b5e0a3b5-1f6c-4a37-bfe6-6b4d64b74b4a"),
            Unit : Units.Meter,
            IsActive : true
        );
}
