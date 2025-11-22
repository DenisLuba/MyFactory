using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/shipments")]
public class ShipmentsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateShipment([FromBody] object dto)
        => Created("", new { shipmentId = "sh-001", status = "created" });

    [HttpGet("{id}")]
    public IActionResult Get(string id)
        => Ok(new 
        { 
            id, 
            customer = "ИП Клиент1", 
            items = new[] { new { specificationId = "sp-001", qty = 10, unitPrice = 550.0 } } 
        });

    [HttpPost("{id}/confirm-payment")]
    public IActionResult ConfirmPayment(string id)
        => Ok(new { id, status = "paid" });
}
