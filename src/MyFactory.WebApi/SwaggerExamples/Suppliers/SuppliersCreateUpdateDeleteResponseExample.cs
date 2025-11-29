using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Suppliers;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public class SuppliersCreateUpdateDeleteResponseExample : IExamplesProvider<SuppliersCreateUpdateDeleteResponse>
{
    public SuppliersCreateUpdateDeleteResponse GetExamples()
        => new(SupplierStatus.Active);
}

