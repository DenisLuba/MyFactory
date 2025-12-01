using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Returns;
using MyFactory.WebApi.SwaggerExamples.Returns;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/returns")]
public class ReturnsController : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(ReturnsListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<ReturnsListResponse>), StatusCodes.Status200OK)]
    public IActionResult GetReturns()
        => Ok(new[]
        {
            new ReturnsListResponse(
                ReturnId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Customer: "ИП \"Клиент1\"",
                ProductName: "Пижама женская",
                Quantity: 2,
                Date: new DateTime(2025, 11, 13),
                Reason: "Брак",
                Status: ReturnStatus.Accepted
            ),
            new ReturnsListResponse(
                ReturnId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Customer: "ООО \"Текстиль\"",
                ProductName: "Футболка детская",
                Quantity: 1,
                Date: new DateTime(2025, 11, 16),
                Reason: "Ошибка поставки",
                Status: ReturnStatus.Accepted
            )
        });

    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(ReturnCardResponseExample))]
    [ProducesResponseType(typeof(ReturnCardResponse), StatusCodes.Status200OK)]
    public IActionResult GetReturnById(Guid id)
        => Ok(new ReturnCardResponse(
            ReturnId: id,
            Customer: "ИП \"Клиент1\"",
            ProductName: "Пижама женская",
            Quantity: 2,
            Date: new DateTime(2025, 11, 13),
            Reason: "Брак",
            Status: ReturnStatus.Accepted,
            Comment: "Замена изделия оформлена"
        ));

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerRequestExample(typeof(ReturnsCreateRequest), typeof(ReturnsCreateRequestExample))]
    [SwaggerResponseExample(201, typeof(ReturnsCreateResponseExample))]
    [ProducesResponseType(typeof(ReturnsCreateResponse), StatusCodes.Status201Created)]
    public IActionResult CreateReturn([FromBody] ReturnsCreateRequest request)
        => Created(
            "",
            new ReturnsCreateResponse(
                ReturnId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Status: ReturnStatus.Accepted
            )
        );
}
