using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Purchases;
using MyFactory.WebApi.SwaggerExamples.Purchases;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/purchases")]
[Produces("application/json")]
public class PurchasesController : ControllerBase
{
    // -------------------------
    //  CREATE PURCHASE REQUEST
    // -------------------------
    [HttpPost("purchase-requests")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(PurchasesCreateRequest), typeof(PurchasesCreateRequestExample))]
    [SwaggerResponseExample(201, typeof(PurchasesCreateResponseExample))]
    [ProducesResponseType(typeof(PurchasesCreateResponse), StatusCodes.Status201Created)]
    public IActionResult CreatePurchase([FromBody] PurchasesCreateRequest request)
        => Created(
            "",
            new PurchasesCreateResponse(
                PurchaseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Status: PurchasesStatus.Created
            )
        );

    // -------------------------
    //  LIST PURCHASE REQUESTS
    // -------------------------
    [HttpGet("requests")]
    [SwaggerResponseExample(200, typeof(PurchasesResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<PurchasesResponse>), StatusCodes.Status200OK)]
    public IActionResult PurchasesList()
        => Ok(
            new[]
            {
                new PurchasesResponse(
                    PurchaseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    CreatedAt: new DateTime(2025, 11, 12),
                    Status: PurchasesStatus.Draft,
                    Items:
                    [
                        new PurchaseResponseItem(
                            MaterialId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                            Qty: 50
                        )
                    ]
                )
            }
        );

    // -------------------------
    //  CONVERT REQUEST → ORDER
    // -------------------------
    [HttpPost("requests/{id}/convert-to-order")]
    [SwaggerResponseExample(200, typeof(PurchasesConvertToOrderResponseExample))]
    [ProducesResponseType(typeof(PurchasesConvertToOrderResponse), StatusCodes.Status200OK)]
    public IActionResult ConvertToOrder(string id)
        => Ok(
            new PurchasesConvertToOrderResponse(
                PurchaseId: Guid.Parse(id),
                Status: PurchasesStatus.Converted
            )
        );
}
