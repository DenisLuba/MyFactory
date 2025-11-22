using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/purchases")]
public class PurchasesController : ControllerBase
{
    [HttpPost("purchase-requests")]
    public IActionResult CreatePurchaseRequest([FromBody] object dto)
        => Created("", new { prId = "pr-001", status = "created" });

    [HttpGet("requests")]
    public IActionResult ListPurchaseRequests()
        => Ok(new[] { new { id = "pr-001", createdAt = "2025-11-12", status = "Draft", items = new[] { new { material = "Ткань Ситец", qty = 50 } } } });

    [HttpPost("requests/{id}/convert-to-order")]
    public IActionResult ConvertToOrder(string id)
        => Ok(new { status = "converted", prId = id });
}
