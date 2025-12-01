using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Shipments;
using MyFactory.WebApi.SwaggerExamples.Shipments;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/shipments")]
public class ShipmentsController : ControllerBase
{
    // GET /api/shipments
    [HttpGet]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(ShipmentsListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<ShipmentsListResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
        => Ok(
            new[]
            {
                new ShipmentsListResponse(
                    ShipmentId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Customer: "ИП Клиент1",
                    ProductName: "Пижама женская",
                    Quantity: 10,
                    Date: new DateTime(2025, 11, 12),
                    TotalAmount: 5500m,
                    Status: ShipmentStatus.Draft
                ),
                new ShipmentsListResponse(
                    ShipmentId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Customer: "ООО \"Текстиль\"",
                    ProductName: "Футболка детская",
                    Quantity: 25,
                    Date: new DateTime(2025, 11, 15),
                    TotalAmount: 4250m,
                    Status: ShipmentStatus.Paid
                )
            }
        );

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
                Status: ShipmentStatus.Draft
            )
        );

    // GET /api/shipments/{id}
    [HttpGet("{id}")]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(ShipmentCardResponseExample))]
    [ProducesResponseType(typeof(ShipmentCardResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
        => Ok(
            new ShipmentCardResponse(
                ShipmentId: id,
                Customer: "ИП Клиент1",
                Date: new DateTime(2025, 11, 12),
                Status: ShipmentStatus.Draft,
                TotalAmount: 5500m,
                Items: new[]
                {
                    new ShipmentItemDto(
                        SpecificationId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                        ProductName: "Пижама женская",
                        Qty: 10,
                        UnitPrice: 550.0m,
                        LineTotal: 5500m
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
                Status: ShipmentStatus.Paid
            )
        );
}
