using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Suppliers;
using MyFactory.WebApi.SwaggerExamples.Suppliers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    /// <summary>Список поставщиков</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SupplierResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SuppliersListResponseExample))]
    public IActionResult List()
        => Ok(new[]
        {
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
        });

    /// <summary>Детальная карточка поставщика</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SupplierResponseExample))]
    public IActionResult Get(Guid id)
        => Ok(
            new SupplierResponse(
                id,
                "ТексМаркет",
                SupplierTypes.Materials,
                SupplierStatus.Active,
                "г. Иваново, ул. Текстильщиков, 12",
                "+7 (900) 111–22–33",
                "info@texmarket.ru"
            ));

    /// <summary>Создать поставщика</summary>
    [HttpPost]
    [ProducesResponseType(typeof(SuppliersCreateUpdateDeleteResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(SuppliersCreateUpdateRequest), typeof(SuppliersCreateUpdateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(SuppliersCreateUpdateDeleteResponseExample))]
    public IActionResult Create([FromBody] SuppliersCreateUpdateRequest request)
        => Created("", new SuppliersCreateUpdateDeleteResponse(SupplierStatus.Created));

    /// <summary>Обновить поставщика</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(SuppliersCreateUpdateDeleteResponse), StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(SuppliersCreateUpdateRequest), typeof(SuppliersCreateUpdateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SuppliersCreateUpdateDeleteResponseExample))]
    public IActionResult Update(Guid id, [FromBody] SuppliersCreateUpdateRequest request)
        => Ok(new SuppliersCreateUpdateDeleteResponse(SupplierStatus.Updated));

    /// <summary>Удалить поставщика</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(SuppliersCreateUpdateDeleteResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SuppliersCreateUpdateDeleteResponseExample))]
    public IActionResult Delete(Guid id)
        => Ok(new SuppliersCreateUpdateDeleteResponse(SupplierStatus.Deleted));
}
