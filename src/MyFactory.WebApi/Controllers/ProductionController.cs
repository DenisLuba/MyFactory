using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Production;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/production")]
public class ProductionController : ControllerBase
{
    [HttpPost("orders")]
    public IActionResult CreateOrder([FromBody] object dto)
        => Created(
            "", 
            new PurchasesCreateOrderResponse(
                OrderId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 
                Status = ProductionStatus.Created
            )
        );

    [HttpGet("orders/{id}")]
    public IActionResult GetOrder(string id)
        => Ok(new ProductionGetOrderStatusResponse
            (
                Id: Guid.Parse(id),
                SpecificationId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                QtyOrdered: 10,
                Allocation: new[] 
                {
                    new Allocation(WorkshopId = "ws-1", QtyAllocated = 8) 
                }           
            )
        );

    [HttpGet("orders/{id}/status")]
    public IActionResult GetOrderStatus(string id)
        => Ok(new ProductionGetOrderStatusResponse
            (
                Id: Guid.Parse(id),
                Status = ProductionStatus.Allocated
            )
        );

    [HttpPost("orders/{id}/allocate")]
    public IActionResult Allocate(string id, [FromBody] object dto)
        => Ok(new ProductionAllocateResponse
            (
                OrderId = Guid.Parse(id),
                Status = ProductionStatus.Allocated
            )
        );

    [HttpPost("orders/{id}/stages")]
    public IActionResult RecordStage(string id, [FromBody] object dto)
        => Ok(new ProductionRecordStageResponse
            (
                OrderId = Guid.Parse(id), 
                Status = ProductionStatus.StageRecorded
            }
        );

    [HttpPost("orders/{id}/assign")]
    public IActionResult AssignWorker(string id, [FromBody] object dto)
        => Ok(new ProductionAssignWorkerResponse
            (
                OrderId = Guid.Parse(id), 
                Status = ProductionStatus.WorkerAssigned
            )
        );
}