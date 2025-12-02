using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Workshops;
using MyFactory.WebApi.SwaggerExamples.Workshops;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/workshops")]
[Produces("application/json")]
public class WorkshopsController : ControllerBase
{
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(WorkshopsListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<WorkshopsListResponse>), StatusCodes.Status200OK)]
    public IActionResult List()
        => Ok(new[]
        {
            new WorkshopsListResponse(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Крой", WorkshopType.Cutting, WorkshopStatus.Active),
            new WorkshopsListResponse(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Пошив", WorkshopType.Sewing, WorkshopStatus.Active),
            new WorkshopsListResponse(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Упаковка", WorkshopType.Packing, WorkshopStatus.Inactive)
        });

    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(WorkshopGetResponseExample))]
    [ProducesResponseType(typeof(WorkshopGetResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
        => Ok(new WorkshopGetResponse(id, "Крой", WorkshopType.Cutting, WorkshopStatus.Active));

    [HttpPost]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(WorkshopCreateRequest), typeof(WorkshopCreateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(WorkshopCreateResponseExample))]
    [ProducesResponseType(typeof(WorkshopCreateResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] WorkshopCreateRequest request)
        => Created(
            string.Empty,
            new WorkshopCreateResponse(Guid.NewGuid(), request.Status)
        );

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(WorkshopUpdateRequest), typeof(WorkshopUpdateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(WorkshopUpdateResponseExample))]
    [ProducesResponseType(typeof(WorkshopUpdateResponse), StatusCodes.Status200OK)]
    public IActionResult Update(Guid id, [FromBody] WorkshopUpdateRequest request)
        => Ok(new WorkshopUpdateResponse(id, request.Status));
}
