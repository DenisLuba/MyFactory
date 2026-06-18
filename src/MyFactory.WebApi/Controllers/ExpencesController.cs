using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Expenses.CreateExpense;
using MyFactory.Application.Features.Expenses.DeleteExpense;
using MyFactory.Application.Features.Expenses.GetExpenses;
using MyFactory.Application.Features.Expenses.UpdateExpense;
using MyFactory.WebApi.Contracts.Expences;
using MyFactory.WebApi.SwaggerExamples.Expences;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/expences")]
[Produces("application/json")]
public class ExpencesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpencesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ExpenseListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ExpenseListResponseExample))]
    public async Task<IActionResult> GetList([FromQuery] DateOnly from, [FromQuery] DateOnly to, [FromQuery] Guid? expenseTypeId = null)
    {
        var items = await _mediator.Send(new GetExpensesQuery(from, to, expenseTypeId));
        var response = items
            .Select(x => new ExpenseListItemResponse(x.Id, x.ExpenseDate, x.ExpenseTypeName, x.Amount, x.Description, x.CreatedBy))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateExpenseResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateExpenseRequest), typeof(CreateExpenseRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateExpenseResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateExpenseRequest req)
    {
        var id = await _mediator.Send(new CreateExpenseCommand(req.ExpenseTypeId, req.ExpenseDate, req.Amount, req.Description));
        return Created(string.Empty, new CreateExpenseResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateExpenseRequest), typeof(UpdateExpenseRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExpenseRequest req)
    {
        await _mediator.Send(new UpdateExpenseCommand(id, req.ExpenseDate, req.ExpenseTypeId, req.Amount, req.Description));
        return NoContent();
    }

    // -------------------------
    //  DELETE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteExpenseCommand(id));
        return NoContent();
    }
}

