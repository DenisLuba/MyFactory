using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Suppliers;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public class SuppliersListResponseExample : IExamplesProvider<IEnumerable<SupplierResponse>>
{
    public IEnumerable<SupplierResponse> GetExamples()
        =>
        [
            new SupplierResponse(
                Guid.NewGuid(),
                "ТексМаркет",
                SupplierTypes.Materials,
                SupplierStatus.Active,
                "г. Иваново, ул. Текстильщиков, 12",
                "+7 (900) 111–22–33",
                "info@texmarket.ru"
            ),
            new SupplierResponse(
                Guid.NewGuid(),
                "Фабрика-Текстиль",
                SupplierTypes.Hardware,
                SupplierStatus.Active,
                "г. Москва, ул. Ленина, 123",
                "+7 (908) 999–22–12",
                "info@texfactory.ru"
            )
        ];
}


