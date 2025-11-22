using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/production")]
public class ProductionController : ControllerBase
{
    [HttpPost("orders")]
    public IActionResult CreateOrder([FromBody] object dto)
        => Created("", new { orderId = "po-001", status = "created" });

    [HttpGet("orders/{id}")]
    public IActionResult GetOrder(string id)
        => Ok(new { id, specificationId = "sp-001", qtyOrdered = 10, allocations = new[] { new { workshopId = "ws-1", qtyAllocated = 8 } } });

    [HttpGet("orders/{id}/status")]
    public IActionResult GetOrderStatus(string id)
        => Ok(new
        {
            id,
            status = "allocated"
        });

    [HttpPost("orders/{id}/allocate")]
    public IActionResult Allocate(string id, [FromBody] object dto)
        => Ok(new { orderId = id, status = "allocated" });

    [HttpPost("orders/{id}/stages")]
    public IActionResult RecordStage(string id, [FromBody] object dto)
        => Ok(new { orderId = id, status = "stage-recorded" });

    [HttpPost("orders/{id}/assign")]
    public IActionResult AssignWorker(string id, [FromBody] object dto)
        => Ok(new { orderId = id, status = "worker-assigned" });
}