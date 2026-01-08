using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.MaterialTypes.CreateMaterialType;
using MyFactory.Application.Features.MaterialTypes.DeactivateMaterialType;
using MyFactory.Application.Features.MaterialTypes.GetMaterialTypeDetails;
using MyFactory.Application.Features.MaterialTypes.GetMaterialTypes;
using MyFactory.Application.Features.MaterialTypes.UpdateMaterialType;
using MyFactory.WebApi.Contracts.MaterialTypes;
using MyFactory.WebApi.SwaggerExamples.MaterialTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/materialtypes")]
[Produces("application/json")]
public class MaterialTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MaterialTypeResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MaterialTypeListResponseExample))]
    public async Task<IActionResult> GetList()
    {
        var dtos = await _mediator.Send(new GetMaterialTypesQuery());
        var response = dtos
            .Select(x => new MaterialTypeResponse(x.Id, x.Name, x.Description))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MaterialTypeResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MaterialTypeDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetMaterialTypeDetailsQuery(id));
        var response = new MaterialTypeResponse(dto.Id, dto.Name, dto.Description);
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateMaterialTypeResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateMaterialTypeRequest), typeof(CreateMaterialTypeRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateMaterialTypeResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateMaterialTypeRequest req)
    {
        var id = await _mediator.Send(new CreateMaterialTypeCommand(req.Name, req.Description));
        return Created(string.Empty, new CreateMaterialTypeResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateMaterialTypeRequest), typeof(UpdateMaterialTypeRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMaterialTypeRequest req)
    {
        await _mediator.Send(new UpdateMaterialTypeCommand(id, req.Name, req.Description));
        return NoContent();
    }

    // -------------------------
    //  DELETE (Deactivate)
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeactivateMaterialTypeCommand(id));
        return NoContent();
    }
}

