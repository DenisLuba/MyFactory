using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.MaterialPurchaseOrders.GetDetails;
using MyFactory.Application.Features.Suppliers.CreateSupplier;
using MyFactory.Application.Features.Suppliers.DeleteSupplier;
using MyFactory.Application.Features.Suppliers.GetSupplierDetails;
using MyFactory.Application.Features.Suppliers.GetSuppliers;
using MyFactory.Application.Features.Suppliers.UpdateSupplier;
using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using MyFactory.WebApi.Contracts.Suppliers;
using MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;
using MyFactory.WebApi.SwaggerExamples.Suppliers;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Common;

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
    [ProducesResponseType(typeof(ListResponse<SupplierListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(SupplierListResponseExample))]
    public async Task<IActionResult> GetList(
        [FromQuery] string? searchName,
        [FromQuery] string? sortBy,
        [FromQuery] bool sortDesc = false,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 30,
        [FromQuery] bool? isActive = null)
    {
        var result = await _mediator.Send(new GetSuppliersQuery(
            searchName,
            sortBy,
            sortDesc,
            skip,
            take,
            isActive));

        var response = new ListResponse<SupplierListItemResponse>(
            Items: [.. result.Items
                .Select(c => new SupplierListItemResponse(
                    Id: c.Id,
                    Name: c.Name,
                    IsActive: c.IsActive))],
            TotalCount: result.TotalCount,
            Take: result.Take,
            Skip: result.Skip,
            HasMore: result.HasMore);
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SupplierDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MaterialPurchaseOrderDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetSupplierDetailsQuery(id));
        var response = new SupplierDetailsResponse(
            Id: dto.Id,
            Name: dto.Name,
            Description: dto.Description,
            Purchases: [.. dto.Purchases.Select(p => new SupplierPurchaseHistoryResponse(
                OrderId: p.OrderId,
                PurchaseNumber: p.PurchaseNumber,
                Date: p.Date,
                Status: p.Status,
                Items: [.. p.Items.Select(i => new SupplierPurchaseHistoryItemResponse(
                    MaterialType: i.MaterialType,
                    MaterialName: i.MaterialName,
                    Qty: i.Qty,
                    UnitPrice: i.UnitPrice
                ))]
            ))]
        );

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
