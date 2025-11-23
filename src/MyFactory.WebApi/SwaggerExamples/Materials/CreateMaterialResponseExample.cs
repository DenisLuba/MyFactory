using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class CreateMaterialResponseExample : IExamplesProvider<CreateMaterialResponse>
{
    public CreateMaterialResponse GetExamples() =>
        new(Status: MaterialStatus.Created);
}
