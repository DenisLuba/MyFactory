using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Suppliers.CreateSupplier;
using MyFactory.Application.Features.Suppliers.DeleteSupplier;
using MyFactory.Application.Features.Suppliers.GetSupplierDetails;
using MyFactory.Application.Features.Suppliers.GetSuppliers;
using MyFactory.Application.Features.Suppliers.UpdateSupplier;
using MyFactory.WebApi.Contracts.Suppliers;
using MyFactory.WebApi.SwaggerExamples.Suppliers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/suppliers")]
[Produces("application/json")]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SupplierListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(SupplierListResponseExample))]
    public async Task<IActionResult> GetList([FromQuery] string? search)
    {
        var dtos = await _mediator.Send(new GetSuppliersQuery(search));
        var response = dtos
            .Select(x => new SupplierListItemResponse(x.Id, x.Name, x.IsActive))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SupplierDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(SupplierDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetSupplierDetailsQuery(id));
        var response = new SupplierDetailsResponse(
            dto.Id,
            dto.Name,
            dto.Description,
            dto.Purchases
                .Select(p => new SupplierPurchaseHistoryResponse(
                    p.OrderId,
                    p.MaterialType,
                    p.MaterialName,
                    p.Qty,
                    p.UnitPrice,
                    p.Date,
                    p.Status))
                .ToList());
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateSupplierResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateSupplierRequest), typeof(CreateSupplierRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateSupplierResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateSupplierRequest req)
    {
        var id = await _mediator.Send(new CreateSupplierCommand(req.Name, req.Description));
        return Created(string.Empty, new CreateSupplierResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(UpdateSupplierRequest), typeof(UpdateSupplierRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupplierRequest req)
    {
        await _mediator.Send(new UpdateSupplierCommand
        {
            SupplierId = id,
            Name = req.Name,
            Description = req.Description
        });
        return Ok();
    }

    // -------------------------
    //  DELETE (DEACTIVATE)
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteSupplierCommand(id));
        return Ok();
    }
}
