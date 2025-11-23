using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class CreateMaterialRequestExample : IExamplesProvider<CreateMaterialRequest>
{
    public CreateMaterialRequest GetExamples() =>
        new(
            Code : "MAT-001",
            Name : "Ткань Ситец",
            MaterialTypeId : Guid.Parse(""),
            Unit : Units.Meter,
            IsActive : true
        );
}
