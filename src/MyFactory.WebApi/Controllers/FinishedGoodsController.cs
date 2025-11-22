using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.FinishedGoods;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/finished-goods")]
public class FinishedGoodsController : ControllerBase
{
    [HttpPost("receipt")]
    public IActionResult Receipt([FromBody] ReceiptFinishedGoodsRequest request)
        => Created("", new ReceiptFinishedGoodsResponse("fg-rc-001", "accepted"));

    [HttpGet]
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
    public IActionResult Move([FromBody] MoveFinishedGoodsRequest request)
        => Ok(new MoveFinishedGoodsResponse("moved"));
}
