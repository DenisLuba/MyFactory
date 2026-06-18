using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.ExpenseTypes.CreateExpenseType;
using MyFactory.Application.Features.ExpenseTypes.DeleteExpenseType;
using MyFactory.Application.Features.ExpenseTypes.GetExpenseTypeDetails;
using MyFactory.Application.Features.ExpenseTypes.GetExpenseTypes;
using MyFactory.Application.Features.ExpenseTypes.UpdateExpenseType;
using MyFactory.WebApi.Contracts.ExpenceTypes;
using MyFactory.WebApi.SwaggerExamples.ExpenceTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/expencetypes")]
[Produces("application/json")]
public class ExpenceTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenceTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ExpenseTypeResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ExpenseTypeListResponseExample))]
    public async Task<IActionResult> GetList()
    {
        var items = await _mediator.Send(new GetExpenseTypesQuery());
        var response = items
            .Select(x => new ExpenseTypeResponse(x.Id, x.Name, x.Description))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ExpenseTypeResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ExpenseTypeDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetExpenseTypeDetailsQuery(id));
        return Ok(new ExpenseTypeResponse(dto.Id, dto.Name, dto.Description));
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateExpenseTypeResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateExpenseTypeRequest), typeof(CreateExpenseTypeRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateExpenseTypeResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateExpenseTypeRequest req)
    {
        var id = await _mediator.Send(new CreateExpenseTypeCommand(req.Name, req.Description));
        return Created(string.Empty, new CreateExpenseTypeResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateExpenseTypeRequest), typeof(UpdateExpenseTypeRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExpenseTypeRequest req)
    {
        await _mediator.Send(new UpdateExpenseTypeCommand(id, req.Name, req.Description));
        return NoContent();
    }

    // -------------------------
    //  DELETE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteExpenseTypeCommand(id));
        return NoContent();
    }
}
