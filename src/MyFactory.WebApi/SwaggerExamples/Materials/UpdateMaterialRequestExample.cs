using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class UpdateMaterialRequestExample : IExamplesProvider<UpdateMaterialRequest>
{
    public UpdateMaterialRequest GetExamples() =>
        new(
            Code: "MAT-001",
            Name: "Ткань Ситец (обновленная)",
            MaterialTypeId: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Unit: Units.Meter,
            IsActive: true
        );
}
