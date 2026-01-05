using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Common.ValueObjects;
using MyFactory.Application.Features.Employees.ActivateEmployee;
using MyFactory.Application.Features.Employees.AddTimesheetEntry;
using MyFactory.Application.Features.Employees.CreateEmployee;
using MyFactory.Application.Features.Employees.DeactivateEmployee;
using MyFactory.Application.Features.Employees.GetEmployeeDetails;
using MyFactory.Application.Features.Employees.GetEmployeeProductionAssignments;
using MyFactory.Application.Features.Employees.GetEmployeeTimesheetDetails;
using MyFactory.Application.Features.Employees.GetTimesheets;
using MyFactory.Application.Features.Employees.RemoveTimesheetEntry;
using MyFactory.Application.Features.Employees.UpdateEmployee;
using MyFactory.Application.Features.Employees.UpdateTimesheetEntry;
using MyFactory.Application.Features.Organization.Employees.GetEmployee;
using MyFactory.WebApi.Contracts.Employees;
using MyFactory.WebApi.SwaggerExamples.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/employees")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(EmployeeListResponseExample))]
    public async Task<IActionResult> GetList([FromQuery] string? search, [FromQuery] EmployeeSortBy sortBy = EmployeeSortBy.FullName, [FromQuery] bool sortDesc = false)
    {
        var items = await _mediator.Send(new GetEmployeesQuery { Search = search, SortBy = sortBy, SortDesc = sortDesc });
        var response = items.Select(x => new EmployeeListItemResponse(x.Id, x.FullName, x.DepartmentName, x.PositionName, x.IsActive)).ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EmployeeDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(EmployeeDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetEmployeeDetailsQuery(id));
        var response = new EmployeeDetailsResponse(
            dto.Id,
            dto.FullName,
            new DepartmentInfoResponse(dto.Department.Id, dto.Department.Name),
            new PositionInfoResponse(dto.Position.Id, dto.Position.Name, dto.Position.DepartmentName),
            dto.Grade,
            dto.RatePerNormHour,
            dto.PremiumPercent,
            dto.HiredAt,
            dto.FiredAt,
            dto.IsActive,
            dto.Contacts.Select(c => new ContactResponse(c.Type, c.Value)).ToList());
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateEmployeeResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateEmployeeRequest), typeof(CreateEmployeeRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateEmployeeResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest req)
    {
        var id = await _mediator.Send(new CreateEmployeeCommand(req.FullName, req.PositionId, req.Grade, req.RatePerNormHour, req.PremiumPercent, req.HiredAt, req.IsActive));
        return Created(string.Empty, new CreateEmployeeResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateEmployeeRequest), typeof(UpdateEmployeeRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest req)
    {
        await _mediator.Send(new UpdateEmployeeCommand(id, req.FullName, req.PositionId, req.Grade, req.RatePerNormHour, req.PremiumPercent, req.HiredAt, req.IsActive));
        return NoContent();
    }

    // -------------------------
    //  ACTIVATE / DEACTIVATE
    // -------------------------
    [HttpPost("{id:guid}/activate")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(ActivateEmployeeRequest), typeof(ActivateEmployeeRequestExample))]
    public async Task<IActionResult> Activate(Guid id, [FromBody] ActivateEmployeeRequest req)
    {
        await _mediator.Send(new ActivateEmployeeCommand(id, req.HiredAt));
        return NoContent();
    }

    [HttpPost("{id:guid}/deactivate")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(DeactivateEmployeeRequest), typeof(DeactivateEmployeeRequestExample))]
    public async Task<IActionResult> Deactivate(Guid id, [FromBody] DeactivateEmployeeRequest req)
    {
        await _mediator.Send(new DeactivateEmployeeCommand(id, req.FiredAt));
        return NoContent();
    }

    // -------------------------
    //  PRODUCTION ASSIGNMENTS
    // -------------------------
    [HttpGet("{id:guid}/assignments")]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeProductionAssignmentResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(EmployeeProductionAssignmentsExample))]
    public async Task<IActionResult> GetAssignments(Guid id)
    {
        var items = await _mediator.Send(new GetEmployeeProductionAssignmentsQuery(id));
        var response = items.Select(x => new EmployeeProductionAssignmentResponse(x.ProductionOrderId, x.ProductionOrderNumber, x.Stage, x.QtyAssigned, x.QtyCompleted)).ToList();
        return Ok(response);
    }

    // -------------------------
    //  TIMESHEETS (AGGREGATED)
    // -------------------------
    [HttpGet("timesheets")]
    [ProducesResponseType(typeof(IReadOnlyList<TimesheetListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(TimesheetListResponseExample))]
    public async Task<IActionResult> GetTimesheets([FromQuery] Guid? employeeId, [FromQuery] Guid? departmentId, [FromQuery] int year, [FromQuery] int month)
    {
        var items = await _mediator.Send(new GetTimesheetsQuery(employeeId, departmentId, new YearMonth(year, month)));
        var response = items.Select(x => new TimesheetListItemResponse(x.EmployeeId, x.EmployeeName, x.DepartmentName, x.TotalHours, x.WorkDays)).ToList();
        return Ok(response);
    }

    // -------------------------
    //  TIMESHEET DETAILS (EMPLOYEE)
    // -------------------------
    [HttpGet("{id:guid}/timesheet")]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeTimesheetEntryResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(EmployeeTimesheetDetailsExample))]
    public async Task<IActionResult> GetEmployeeTimesheet(Guid id, [FromQuery] int year, [FromQuery] int month)
    {
        var items = await _mediator.Send(new GetEmployeeTimesheetDetailsQuery(id, new YearMonth(year, month)));
        var response = items.Select(x => new EmployeeTimesheetEntryResponse(x.EntryId, x.Date, x.Hours, x.Comment)).ToList();
        return Ok(response);
    }

    // -------------------------
    //  TIMESHEET ENTRY CRUD
    // -------------------------
    [HttpPost("{id:guid}/timesheet")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AddTimesheetEntryResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(AddTimesheetEntryRequest), typeof(AddTimesheetEntryRequestExample))]
    [SwaggerResponseExample(201, typeof(AddTimesheetEntryResponseExample))]
    public async Task<IActionResult> AddTimesheetEntry(Guid id, [FromBody] AddTimesheetEntryRequest req)
    {
        var entryId = await _mediator.Send(new AddTimesheetEntryCommand(id, req.Date, req.Hours, req.Comment));
        return Created(string.Empty, new AddTimesheetEntryResponse(entryId));
    }

    [HttpPut("timesheet/{entryId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateTimesheetEntryRequest), typeof(UpdateTimesheetEntryRequestExample))]
    public async Task<IActionResult> UpdateTimesheetEntry(Guid entryId, [FromBody] UpdateTimesheetEntryRequest req)
    {
        await _mediator.Send(new UpdateTimesheetEntryCommand(entryId, req.Hours, req.Comment));
        return NoContent();
    }

    [HttpDelete("timesheet/{entryId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveTimesheetEntry(Guid entryId)
    {
        await _mediator.Send(new RemoveTimesheetEntryCommand(entryId));
        return NoContent();
    }
}

