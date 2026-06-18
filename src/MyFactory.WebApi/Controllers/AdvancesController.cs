using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Advances.AddCashAdvanceAmount;
using MyFactory.Application.Features.Advances.CloseCashAdvance;
using MyFactory.Application.Features.Advances.CreateCashAdvance;
using MyFactory.Application.Features.Advances.CreateCashAdvanceExpense;
using MyFactory.Application.Features.Advances.CreateCashAdvanceReturn;
using MyFactory.Application.Features.Advances.GetCashAdvances;
using MyFactory.WebApi.Contracts.Advances;
using MyFactory.WebApi.SwaggerExamples.Advances;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/advances")]
[Produces("application/json")]
public class AdvancesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdvancesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CashAdvanceListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(CashAdvanceListExample))]
    public async Task<IActionResult> GetList([FromQuery] DateOnly? from, [FromQuery] DateOnly? to, [FromQuery] Guid? employeeId)
    {
        var result = await _mediator.Send(new GetCashAdvancesQuery(from, to, employeeId));

        var response = result
            .Select(a => new CashAdvanceListItemResponse(
                a.Id,
                a.IssueDate,
                a.EmployeeName,
                a.IssuedAmount,
                a.SpentAmount,
                a.ReturnedAmount,
                a.Balance,
                a.IsClosed))
            .ToList();

        return Ok(response);
    }

    // -------------------------
    //  ISSUE
    // -------------------------
    [HttpPost("issue")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateCashAdvanceResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateCashAdvanceRequest), typeof(CreateCashAdvanceRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateCashAdvanceResponseExample))]
    public async Task<IActionResult> Issue([FromBody] CreateCashAdvanceRequest req)
    {
        var id = await _mediator.Send(new CreateCashAdvanceCommand(req.EmployeeId, req.IssueDate, req.Amount, req.Description));
        return Created(string.Empty, new CreateCashAdvanceResponse(id));
    }

    // -------------------------
    //  ADD AMOUNT
    // -------------------------
    [HttpPost("{id:guid}/amount")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(AddCashAdvanceAmountRequest), typeof(AddCashAdvanceAmountRequestExample))]
    public async Task<IActionResult> AddAmount(Guid id, [FromBody] AddCashAdvanceAmountRequest req)
    {
        await _mediator.Send(new AddCashAdvanceAmountCommand(id, req.IssueDate, req.Amount));
        return NoContent();
    }

    // -------------------------
    //  ADD EXPENSE
    // -------------------------
    [HttpPost("{id:guid}/expenses")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateCashAdvanceExpenseResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateCashAdvanceExpenseRequest), typeof(CreateCashAdvanceExpenseRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateCashAdvanceExpenseResponseExample))]
    public async Task<IActionResult> AddExpense(Guid id, [FromBody] CreateCashAdvanceExpenseRequest req)
    {
        var expenseId = await _mediator.Send(new CreateCashAdvanceExpenseCommand(id, req.ExpenseDate, req.Amount, req.Description));
        return Created(string.Empty, new CreateCashAdvanceExpenseResponse(expenseId));
    }

    // -------------------------
    //  ADD RETURN
    // -------------------------
    [HttpPost("{id:guid}/returns")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateCashAdvanceReturnResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateCashAdvanceReturnRequest), typeof(CreateCashAdvanceReturnRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateCashAdvanceReturnResponseExample))]
    public async Task<IActionResult> AddReturn(Guid id, [FromBody] CreateCashAdvanceReturnRequest req)
    {
        var returnId = await _mediator.Send(new CreateCashAdvanceReturnCommand(id, req.ReturnDate, req.Amount, req.Description));
        return Created(string.Empty, new CreateCashAdvanceReturnResponse(returnId));
    }

    // -------------------------
    //  CLOSE
    // -------------------------
    [HttpPost("{id:guid}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Close(Guid id)
    {
        await _mediator.Send(new CloseCashAdvanceCommand(id));
        return NoContent();
    }
}

