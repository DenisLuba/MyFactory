using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Positions.CreatePositions;
using MyFactory.Application.Features.Positions.GetPositionDetails;
using MyFactory.Application.Features.Positions.GetPositions;
using MyFactory.Application.Features.Positions.UpdatePosition;
using MyFactory.WebApi.Contracts.Positions;
using MyFactory.WebApi.SwaggerExamples.Positions;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/positions")]
[Produces("application/json")]
public class PositionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PositionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PositionListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(PositionListResponseExample))]
    public async Task<IActionResult> GetList(
        [FromQuery] Guid? departmentId,
        [FromQuery] bool includeInactive = false,
        [FromQuery] PositionSortBy sortBy = PositionSortBy.Name,
        [FromQuery] bool sortDesc = false)
    {
        var dtos = await _mediator.Send(new GetPositionsQuery(departmentId, includeInactive, sortBy, sortDesc));
        var response = dtos
            .Select(x => new PositionListItemResponse(
                x.Id,
                x.Name,
                x.Code,
                x.DepartmentId,
                x.DepartmentName,
                x.IsActive))
            .ToList();

        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PositionDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(PositionDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetPositionDetailsQuery(id));
        var response = new PositionDetailsResponse(
            dto.Id,
            dto.Name,
            dto.Code,
            dto.DepartmentId,
            dto.DepartmentName,
            dto.BaseNormPerHour,
            dto.BaseRatePerNormHour,
            dto.DefaultPremiumPercent,
            dto.CanCut,
            dto.CanSew,
            dto.CanPackage,
            dto.CanHandleMaterials,
            dto.IsActive);
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreatePositionResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreatePositionRequest), typeof(CreatePositionRequestExample))]
    [SwaggerResponseExample(201, typeof(CreatePositionResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreatePositionRequest req)
    {
        var id = await _mediator.Send(new CreatePositionCommand(
            req.Name,
            req.Code,
            req.DepartmentId,
            req.BaseNormPerHour,
            req.BaseRatePerNormHour,
            req.DefaultPremiumPercent,
            req.CanCut,
            req.CanSew,
            req.CanPackage,
            req.CanHandleMaterials));

        return Created(string.Empty, new CreatePositionResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdatePositionRequest), typeof(UpdatePositionRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePositionRequest req)
    {
        await _mediator.Send(new UpdatePositionCommand(
            id,
            req.Name,
            req.Code,
            req.DepartmentId,
            req.BaseNormPerHour,
            req.BaseRatePerNormHour,
            req.DefaultPremiumPercent,
            req.CanCut,
            req.CanSew,
            req.CanPackage,
            req.CanHandleMaterials,
            req.IsActive));

        return NoContent();
    }
}
