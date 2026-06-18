using MyFactory.WebApi.Contracts.ProductTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductTypes;

public sealed class CreateProductTypeResponseExample : IExamplesProvider<CreateProductTypeResponse>
{
    public CreateProductTypeResponse GetExamples() => new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"));
}
