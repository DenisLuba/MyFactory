using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Departments.ActivateDepartments;
using MyFactory.Application.Features.Departments.CreateDepartment;
using MyFactory.Application.Features.Departments.DeactivateDepartments;
using MyFactory.Application.Features.Departments.GetDepartmentDetails;
using MyFactory.Application.Features.Departments.GetDepartments;
using MyFactory.Application.Features.Departments.UpdateDepartment;
using MyFactory.WebApi.Contracts.Departments;
using MyFactory.WebApi.SwaggerExamples.Departments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/departments")]
[Produces("application/json")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DepartmentListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(DepartmentListResponseExample))]
    public async Task<IActionResult> GetList([FromQuery] bool includeInactive = false)
    {
        var result = await _mediator.Send(new GetDepartmentsQuery(includeInactive));
        var response = result
            .Select(x => new DepartmentListItemResponse(x.Id, x.Code, x.Name, x.Type, x.IsActive))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DepartmentDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(DepartmentDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetDepartmentDetailsQuery(id));
        var response = new DepartmentDetailsResponse(dto.Id, dto.Code, dto.Name, dto.Type, dto.IsActive);
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateDepartmentResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateDepartmentRequest), typeof(CreateDepartmentRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateDepartmentResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest req)
    {
        var id = await _mediator.Send(new CreateDepartmentCommand(req.Name, req.Code, req.Type));
        return Created(string.Empty, new CreateDepartmentResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateDepartmentRequest), typeof(UpdateDepartmentRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentRequest req)
    {
        await _mediator.Send(new UpdateDepartmentCommand(id, req.Name, req.Code, req.Type, req.IsActive));
        return NoContent();
    }

    // -------------------------
    //  ACTIVATE
    // -------------------------
    [HttpPost("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _mediator.Send(new ActivateDepartmentCommand(id));
        return NoContent();
    }

    // -------------------------
    //  DEACTIVATE
    // -------------------------
    [HttpPost("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _mediator.Send(new DeactivateDepartmentCommand(id));
        return NoContent();
    }
}

