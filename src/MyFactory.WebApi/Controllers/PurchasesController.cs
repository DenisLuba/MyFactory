using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/purchases")]
public class PurchasesController : ControllerBase
{
    [HttpPost("purchase-requests")]
    public IActionResult CreatePurchase([FromBody] object dto)
        => Created
        (
            "", 
            new PurchasesCreateResponse
            (
                PurchaseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Status = PurchasesStatus.Created
            ) 
        );

    [HttpGet("requests")]
    public IActionResult PurchasesList()
        => Ok(
            new[] 
            { 
                new PurchaseResponse
                (
                    PurchaseId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", 
                    CreatedAt = "2025-11-12", 
                    Status = PurchasesStatus.Draft,
                    Items = new[] { new Item(MaterialId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"), Qty = 50) } 
                ) 
            }
        );

    [HttpPost("requests/{id}/convert-to-order")]
    public IActionResult ConvertToOrder(string id)
        => Ok(new PurchasesConvertToOrder(Status = PurchasesStatus.Converted, PurchaseId = Guid.Parse(id)));
}
