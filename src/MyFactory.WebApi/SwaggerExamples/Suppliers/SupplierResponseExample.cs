using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Suppliers;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public class SupplierResponseExample : IExamplesProvider<SupplierResponse>
{
    public SupplierResponse GetExamples()
        => new(
            Id: Guid.NewGuid(),
            Name: "ТексМаркет",
            SupplierType: SupplierTypes.Materials,
            Status: SupplierStatus.Active,
            Address: "г. Иваново, ул. Текстильщиков, 12",
            Phone: "+7 (900) 111–22–33",
            Email: "info@texmarket.ru"
        );
}

