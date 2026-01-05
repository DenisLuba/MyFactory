using MyFactory.WebApi.Contracts.Suppliers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public sealed class CreateSupplierResponseExample : IExamplesProvider<CreateSupplierResponse>
{
    public CreateSupplierResponse GetExamples() => new(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffff0001"));
}
