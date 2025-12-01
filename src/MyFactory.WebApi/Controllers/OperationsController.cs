using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Operations;
using MyFactory.WebApi.SwaggerExamples.Operations;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/operations")]
[Produces("application/json")]
public class OperationsController : ControllerBase
{
    private static readonly Guid CutOperationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid SewOperationId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private static readonly Guid PackOperationId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

    private static readonly IReadOnlyList<OperationCardResponse> Operations = new List<OperationCardResponse>
    {
        new(CutOperationId, "OPR-001", "Раскрой ткани", "Раскрой", 12.5, 180.0m),
        new(SewOperationId, "OPR-002", "Пошив основы", "Пошив", 35.0, 520.0m),
        new(PackOperationId, "OPR-003", "Финальная упаковка", "Упаковка", 8.0, 95.0m)
    };

    // GET /api/operations
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(OperationListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<OperationListResponse>), StatusCodes.Status200OK)]
    public IActionResult List()
        => Ok(Operations.Select(MapToList));

    // GET /api/operations/{id}
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(OperationCardResponseExample))]
    [ProducesResponseType(typeof(OperationCardResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
    {
        var operation = Operations.FirstOrDefault(o => o.Id == id) ?? Operations.First();
        return Ok(operation);
    }

    // PUT /api/operations/{id}
    [HttpPut("{id}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(OperationUpdateRequest), typeof(OperationUpdateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(OperationUpdateResponseExample))]
    [ProducesResponseType(typeof(OperationUpdateResponse), StatusCodes.Status200OK)]
    public IActionResult Update(Guid id, [FromBody] OperationUpdateRequest request)
    {
        var response = new OperationUpdateResponse(id, "Updated");
        return Ok(response);
    }

    private static OperationListResponse MapToList(OperationCardResponse source)
        => new(source.Id, source.Code, source.Name, source.OperationType, source.Minutes, source.Cost);
}
