using System;
using System.Collections.Generic;
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
    [SwaggerResponseExample(200, typeof(WarehousesListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<WarehousesListResponse>), StatusCodes.Status200OK)]
    public IActionResult List()
        => Ok(new[]
        {
            new WarehousesListResponse(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "ST-001",
                "Основной склад",
                WarehouseType.Materials,
                WarehouseStatus.Active
            ),
            new WarehousesListResponse(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "ST-002",
                "Склад фурнитуры",
                WarehouseType.Materials,
                WarehouseStatus.Active
            ),
            new WarehousesListResponse(
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                "ST-003",
                "Готовая продукция",
                WarehouseType.FinishedGoods,
                WarehouseStatus.Active
            )
        });

    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(200, typeof(WarehousesGetResponseExample))]
    [ProducesResponseType(typeof(WarehousesGetResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
        => Ok(
            new WarehousesGetResponse(
                id,
                "ST-001",
                "Основной склад",
                WarehouseType.Materials,
                "ул. Заводская, 1",
                WarehouseStatus.Active
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
                dto.Status
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
                dto.Status
            )
        );

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Delete(Guid id) => NoContent();
}
