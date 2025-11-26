using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shipments;
using MyFactory.WebApi.SwaggerExamples.Shipments;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/shipments")]
public class ShipmentsController : ControllerBase
{
    // POST /api/shipments
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerRequestExample(typeof(ShipmentsCreateRequest), typeof(ShipmentsCreateRequestExample))]
    [SwaggerResponseExample(201, typeof(ShipmentsCreateResponseExample))]
    [ProducesResponseType(typeof(ShipmentsCreateResponse), StatusCodes.Status201Created)]
    public IActionResult CreateShipment([FromBody] ShipmentsCreateRequest dto)
        => Created(
            "",
            new ShipmentsCreateResponse(
                ShipmentId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Status: ShipmentsStatus.Created
            )
        );

    // GET /api/shipments/{id}
    [HttpGet("{id}")]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(ShipmentsGetResponseExample))]
    [ProducesResponseType(typeof(ShipmentsGetResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
        => Ok(
            new ShipmentsGetResponse(
                Id: id,
                Customer: "ИП Клиент1",
                Items: new[]
                {
                    new ShipmentItemDto(
                        SpecificationId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                        Qty: 10,
                        UnitPrice: 550.0m
                    )
                }
            )
        );

    // POST /api/shipments/{id}/confirm-payment
    [HttpPost("{id}/confirm-payment")]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(ShipmentsConfirmPaymentResponseExample))]
    [ProducesResponseType(typeof(ShipmentsConfirmPaymentResponse), StatusCodes.Status200OK)]
    public IActionResult ConfirmPayment(Guid id)
        => Ok(
            new ShipmentsConfirmPaymentResponse(
                ShipmentId: id,
                Status: ShipmentsStatus.Paid
            )
        );
}
