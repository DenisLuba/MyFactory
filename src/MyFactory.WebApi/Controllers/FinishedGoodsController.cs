using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.FinishedGoods;
using MyFactory.WebApi.SwaggerExamples.FinishedGoods;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/finished-goods")]
public class FinishedGoodsController : ControllerBase
{
    [HttpPost("receipt")]
    [SwaggerRequestExample(typeof(ReceiptFinishedGoodsRequest), typeof(ReceiptFinishedGoodsRequestExample))]
    [SwaggerResponseExample(201, typeof(ReceiptFinishedGoodsResponseExample))]
    public IActionResult Receipt([FromBody] ReceiptFinishedGoodsRequest request)
        => Created("", new ReceiptFinishedGoodsResponse(Guid.Parse("11111111-1111-1111-1111-111111111222"), FinishedGoodsStatus.Accepted));

    [HttpGet]
    [SwaggerResponseExample(200, typeof(FinishedGoodsInventoryResponseExample))]
    public IActionResult Inventory()
        => Ok(new[]
        {
        new FinishedGoodsInventoryResponse(
            Guid.Parse("sp-001"),
            Guid.Parse("st-001"),
            20,
            444.0m
        )
        });

    [HttpPost("move")]
    [SwaggerRequestExample(typeof(MoveFinishedGoodsRequest), typeof(MoveFinishedGoodsRequestExample))]
    [SwaggerResponseExample(200, typeof(MoveFinishedGoodsResponseExample))]
    public IActionResult Move([FromBody] MoveFinishedGoodsRequest request)
        => Ok(new MoveFinishedGoodsResponse(FinishedGoodsStatus.Moved));
}
