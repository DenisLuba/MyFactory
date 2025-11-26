using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Warehouses;
using MyFactory.WebApi.SwaggerExamples.Warehouses;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/warehouses")]
[Produces("application/json")]
public class WarehousesController : ControllerBase
{
    [HttpGet]
    [SwaggerResponseExample(200, typeof(WarehousesGetResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<WarehousesGetResponse>), StatusCodes.Status200OK)]
    public IActionResult List()
        => Ok(new[]
        {
            new WarehousesGetResponse(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Основной склад",
                WarehouseType.Materials,
                "ул. Заводская, 1"
            ),
            new WarehousesGetResponse(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "Склад ГП",
                WarehouseType.FinishedGoods,
                "ул. Заводская, 2"
            )
        });

    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(200, typeof(WarehousesGetResponseExample))]
    [ProducesResponseType(typeof(WarehousesGetResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
        => Ok(
            new WarehousesGetResponse(
                id,
                "Основной склад",
                WarehouseType.Materials,
                "ул. Заводская, 1"
            )
        );

    [HttpPost]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(WarehousesCreateRequest), typeof(WarehousesCreateRequestExample))]
    [SwaggerResponseExample(201, typeof(WarehousesCreateResponseExample))]
    [ProducesResponseType(typeof(WarehousesCreateResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] WarehousesCreateRequest dto)
        => Created(
            "",
            new WarehousesCreateResponse(
                Guid.NewGuid(),
                WarehouseStatus.Created
            )
        );

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(WarehousesUpdateRequest), typeof(WarehousesUpdateRequestExample))]
    [SwaggerResponseExample(200, typeof(WarehousesUpdateResponseExample))]
    [ProducesResponseType(typeof(WarehousesUpdateResponse), StatusCodes.Status200OK)]
    public IActionResult Update(Guid id, [FromBody] WarehousesUpdateRequest dto)
        => Ok(
            new WarehousesUpdateResponse(
                id,
                WarehouseStatus.Updated
            )
        );
}
