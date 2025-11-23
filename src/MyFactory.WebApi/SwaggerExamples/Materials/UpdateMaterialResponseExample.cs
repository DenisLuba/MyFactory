using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class UpdateMaterialResponseExample : IExamplesProvider<UpdateMaterialResponse>
{
    public UpdateMaterialResponse GetExamples() =>
        new(
            Status: MaterialStatus.Updated,
            Id: Guid.Parse("11111111-1111-1111-1111-111111111111")
        );
}
