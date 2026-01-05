using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Common.ValueObjects;
using MyFactory.Application.Features.Finance.AdjustPayrollAccrual;
using MyFactory.Application.Features.Finance.CalculateDailyPayrollAccrual;
using MyFactory.Application.Features.Finance.CalculateDailyPayrollAccrualsForPeriod;
using MyFactory.Application.Features.Finance.CreatePayrollPayment;
using MyFactory.Application.Features.Finance.GetEmployeePayrollAccruals;
using MyFactory.Application.Features.Finance.GetPayrollAccruals;
using MyFactory.WebApi.Contracts.Finance;
using MyFactory.WebApi.SwaggerExamples.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/finance")]
[Produces("application/json")]
public class FinanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public FinanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  PAYROLL ACCRUALS LIST
    // -------------------------
    [HttpGet("payroll/accruals")]
    [ProducesResponseType(typeof(IReadOnlyList<PayrollAccrualListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(PayrollAccrualListResponseExample))]
    public async Task<IActionResult> GetPayrollAccruals(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to,
        [FromQuery] Guid? employeeId,
        [FromQuery] Guid? departmentId)
    {
        var items = await _mediator.Send(new GetPayrollAccrualsQuery(from, to, employeeId, departmentId));
        var response = items.Select(x => new PayrollAccrualListItemResponse(
            x.EmployeeId,
            x.EmployeeName,
            x.TotalHours,
            x.QtyPlanned,
            x.QtyProduced,
            x.QtyExtra,
            x.BaseAmount,
            x.PremiumAmount,
            x.TotalAmount,
            x.PaidAmount,
            x.RemainingAmount)).ToList();
        return Ok(response);
    }

    // -------------------------
    //  PAYROLL ACCRUAL DETAILS BY EMPLOYEE
    // -------------------------
    [HttpGet("payroll/employees/{employeeId:guid}/accruals")]
    [ProducesResponseType(typeof(EmployeePayrollAccrualDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(EmployeePayrollAccrualDetailsResponseExample))]
    public async Task<IActionResult> GetEmployeePayrollAccruals(
        Guid employeeId,
        [FromQuery] int year,
        [FromQuery] int month)
    {
        var dto = await _mediator.Send(new GetEmployeePayrollAccrualsQuery(employeeId, new YearMonth(year, month)));
        var response = new EmployeePayrollAccrualDetailsResponse(
            dto.EmployeeId,
            dto.EmployeeName,
            dto.PositionName,
            new YearMonthResponse(dto.Period.Year, dto.Period.Month),
            dto.TotalBaseAmount,
            dto.TotalPremiumAmount,
            dto.TotalAmount,
            dto.PaidAmount,
            dto.RemainingAmount,
            dto.Days.Select(d => new EmployeePayrollAccrualDailyResponse(
                d.AccrualId,
                d.Date,
                d.HoursWorked,
                d.QtyPlanned,
                d.QtyProduced,
                d.QtyExtra,
                d.BaseAmount,
                d.PremiumAmount,
                d.TotalAmount)).ToList());
        return Ok(response);
    }

    // -------------------------
    //  CALCULATE DAILY ACCRUAL
    // -------------------------
    [HttpPost("payroll/accruals/calculate/daily")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(CalculateDailyPayrollAccrualRequest), typeof(CalculateDailyPayrollAccrualRequestExample))]
    public async Task<IActionResult> CalculateDailyAccrual([FromBody] CalculateDailyPayrollAccrualRequest req)
    {
        await _mediator.Send(new CalculateDailyPayrollAccrualCommand(req.EmployeeId, req.Date));
        return NoContent();
    }

    // -------------------------
    //  CALCULATE PERIOD ACCRUALS
    // -------------------------
    [HttpPost("payroll/accruals/calculate/period")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(CalculatePayrollAccrualsForPeriodRequest), typeof(CalculatePayrollAccrualsForPeriodRequestExample))]
    public async Task<IActionResult> CalculatePeriod([FromBody] CalculatePayrollAccrualsForPeriodRequest req)
    {
        await _mediator.Send(new CalculatePayrollAccrualsForPeriodCommand(new YearMonth(req.Year, req.Month)));
        return NoContent();
    }

    // -------------------------
    //  ADJUST ACCRUAL
    // -------------------------
    [HttpPost("payroll/accruals/{accrualId:guid}/adjust")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(AdjustPayrollAccrualRequest), typeof(AdjustPayrollAccrualRequestExample))]
    public async Task<IActionResult> Adjust(Guid accrualId, [FromBody] AdjustPayrollAccrualRequest req)
    {
        await _mediator.Send(new AdjustPayrollAccrualCommand(accrualId, req.BaseAmount, req.PremiumAmount, req.Reason));
        return NoContent();
    }

    // -------------------------
    //  PAYROLL PAYMENT
    // -------------------------
    [HttpPost("payroll/payments")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreatePayrollPaymentResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreatePayrollPaymentRequest), typeof(CreatePayrollPaymentRequestExample))]
    [SwaggerResponseExample(201, typeof(CreatePayrollPaymentResponseExample))]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePayrollPaymentRequest req)
    {
        var id = await _mediator.Send(new CreatePayrollPaymentCommand(req.EmployeeId, req.PaymentDate, req.Amount));
        return Created(string.Empty, new CreatePayrollPaymentResponse(id));
    }
}
