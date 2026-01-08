using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Units.AddUnit;
using MyFactory.Application.Features.Units.GetUnits;
using MyFactory.Application.Features.Units.RemoveUnit;
using MyFactory.Application.Features.Units.UpdateUnit;
using MyFactory.WebApi.Contracts.Units;
using MyFactory.WebApi.SwaggerExamples.Units;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/units")]
[Produces("application/json")]
public class UnitsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnitsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UnitResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(GetUnitsResponseExample))]
    public async Task<IActionResult> GetList()
    {
        var dtos = await _mediator.Send(new GetUnitsQuery());
        var response = dtos.Select(u => new UnitResponse(u.Id, u.Code, u.Name)).ToList();
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AddUnitResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(AddUnitRequest), typeof(AddUnitRequestExample))]
    [SwaggerResponseExample(201, typeof(AddUnitResponseExample))]
    public async Task<IActionResult> Create([FromBody] AddUnitRequest req)
    {
        var id = await _mediator.Send(new AddUnitCommand(req.Code, req.Name));
        return Created($"/api/units/{id}", new AddUnitResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateUnitRequest), typeof(UpdateUnitRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUnitRequest req)
    {
        await _mediator.Send(new UpdateUnitCommand(id, req.Code, req.Name));
        return NoContent();
    }

    // -------------------------
    //  DELETE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new RemoveUnitCommand(id));
        return NoContent();
    }
}

