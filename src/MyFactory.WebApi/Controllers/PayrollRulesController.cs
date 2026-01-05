using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.PayrollRules.CreatePayrollRule;
using MyFactory.Application.Features.PayrollRules.DeletePayrollRule;
using MyFactory.Application.Features.PayrollRules.GetPayrollRuleDetails;
using MyFactory.Application.Features.PayrollRules.GetPayrollRules;
using MyFactory.Application.Features.PayrollRules.UpdatePayrollRule;
using MyFactory.WebApi.Contracts.PayrollRules;
using MyFactory.WebApi.SwaggerExamples.PayrollRules;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/payroll-rules")]
[Produces("application/json")]
public class PayrollRulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PayrollRulesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PayrollRuleResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(PayrollRuleListResponseExample))]
    public async Task<IActionResult> GetList()
    {
        var dtos = await _mediator.Send(new GetPayrollRulesQuery());
        var response = dtos
            .Select(x => new PayrollRuleResponse(x.Id, x.EffectiveFrom, x.PremiumPercent, x.Description))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PayrollRuleResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(PayrollRuleDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetPayrollRuleDetailsQuery(id));
        return Ok(new PayrollRuleResponse(dto.Id, dto.EffectiveFrom, dto.PremiumPercent, dto.Description));
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreatePayrollRuleResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreatePayrollRuleRequest), typeof(CreatePayrollRuleRequestExample))]
    [SwaggerResponseExample(201, typeof(CreatePayrollRuleResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreatePayrollRuleRequest req)
    {
        var id = await _mediator.Send(new CreatePayrollRuleCommand(req.EffectiveFrom, req.PremiumPercent, req.Description));
        return Created(string.Empty, new CreatePayrollRuleResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdatePayrollRuleRequest), typeof(UpdatePayrollRuleRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePayrollRuleRequest req)
    {
        await _mediator.Send(new UpdatePayrollRuleCommand(id, req.EffectiveFrom, req.PremiumPercent, req.Description));
        return NoContent();
    }

    // -------------------------
    //  DELETE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeletePayrollRuleCommand(id));
        return NoContent();
    }
}
