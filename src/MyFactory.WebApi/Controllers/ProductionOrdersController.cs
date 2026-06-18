using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.DTOs.ProductionOrders;
using MyFactory.Application.Features.ProductionOrders.AddProductionStageEmployee;
using MyFactory.Application.Features.ProductionOrders.CancelProductionOrder;
using MyFactory.Application.Features.ProductionOrders.StartNextProductionStage;
using MyFactory.Application.Features.ProductionOrders.CreateProductionOrder;
using MyFactory.Application.Features.ProductionOrders.DeleteProductionOrder;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrderDetails;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrderMaterialIssueDetails;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrderMaterials;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrders;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrdersBySalesOrder;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrderShipments;
using MyFactory.Application.Features.ProductionOrders.GetAvailableProductionStageEmployees;
using MyFactory.Application.Features.ProductionOrders.GetProductionStageEmployees;
using MyFactory.Application.Features.ProductionOrders.GetProductionStages;
using MyFactory.Application.Features.ProductionOrders.RegisterSewingOperation;
using MyFactory.Application.Features.ProductionOrders.IssueMaterialsToProduction;
using MyFactory.Application.Features.ProductionOrders.RemoveProductionStageEmployee;
using MyFactory.Application.Features.ProductionOrders.ShipFinishedGoods;
using MyFactory.Application.Features.ProductionOrders.StartProductionStage;
using MyFactory.Application.Features.ProductionOrders.UpdateProductionOrder;
using MyFactory.Application.Features.ProductionOrders.UpdateProductionStageEmployee;
using MyFactory.Domain.Entities.Production;
using MyFactory.WebApi.Contracts.Common;
using MyFactory.WebApi.Contracts.ProductionOrders;
using MyFactory.WebApi.Contracts.SalesOrders;
using MyFactory.WebApi.SwaggerExamples.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/production-orders")]
[Produces("application/json")]
public class ProductionOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductionOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(ListResponse<ProductionOrderListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderListResponseExample))]
    public async Task<IActionResult> GetList(
        [FromQuery] Guid? searchSaleOrderId = null,
        [FromQuery] string? searchProductionOrderNumber = null,
        [FromQuery] string? searchSaleOrderNumber = null,
        [FromQuery] string? searchCustomerName = null,
        [FromQuery] string? searchProductName = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = false,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 30,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? status = null)
    {
        if (fromDate.HasValue)
        {
            var local = DateTime.SpecifyKind(fromDate.Value.Date, DateTimeKind.Local);
            fromDate = local.ToUniversalTime();
        }

        if (toDate.HasValue)
        {
            var localEnd = DateTime.SpecifyKind(toDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Local);
            toDate = localEnd.ToUniversalTime();
        }

        var dtos = await _mediator.Send(new GetProductionOrdersQuery(
            SearchSaleOrderId: searchSaleOrderId,
            SearchProductionOrderNumber: searchProductionOrderNumber,
            SearchSaleOrderNumber: searchSaleOrderNumber,
            SearchCustomerName: searchCustomerName,
            SearchProductName: searchProductName,
            SortBy: sortBy,
            SortDesc: sortDesc,
            Skip: skip,
            Take: take,
            FromDate: fromDate,
            ToDate: toDate,
            Status: GetProductionOrderStatusFromString(status)));

        var response = new ListResponse<ProductionOrderListItemResponse>(
            Items: [.. dtos.Items
                .Select(x => new ProductionOrderListItemResponse(
                    x.Id,
                    x.CustomerName,
                    x.ProductionOrderNumber,
                    x.SalesOrderNumber,
                    x.ProductName,
                    x.QtyPlanned,
                    x.QtyFinished,
                    x.Status))],
            TotalCount: dtos.TotalCount,
            Take: dtos.Take,
            Skip: dtos.Skip,
            HasMore: dtos.HasMore);

        return Ok(response);
    }

    private static ProductionOrderStatus? GetProductionOrderStatusFromString(string? status)
    {
        return status switch
        {
            "New" => ProductionOrderStatus.New,
            "MaterialIssued" => ProductionOrderStatus.MaterialIssued,
            "Cutting" => ProductionOrderStatus.Cutting,
            "Sewing" => ProductionOrderStatus.Sewing,
            "Packaging" => ProductionOrderStatus.Packaging,
            "Finished" => ProductionOrderStatus.Finished,
            "Cancelled" => ProductionOrderStatus.Cancelled,
            _ => null
        };
    }

    // -------------------------
    //  LIST BY SALES ORDER
    // -------------------------
    [HttpGet("sales-order/{salesOrderId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionOrderListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderListResponseExample))]
    public async Task<IActionResult> GetBySalesOrder(Guid salesOrderId)
    {
        var dtos = await _mediator.Send(new GetProductionOrdersBySalesOrderQuery(salesOrderId));
        var response = dtos
            .Select(x => new ProductionOrderListItemResponse(
                x.Id,
                x.CustomerName,
                x.ProductionOrderNumber,
                x.SalesOrderNumber,
                x.ProductName,
                x.QtyPlanned,
                x.QtyFinished,
                x.Status))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductionOrderDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetProductionOrderDetailsQuery(id));
        var response = new ProductionOrderDetailsResponse(
            dto.Id,
            dto.ProductionOrderNumber,
            dto.SalesOrderId,
            dto.SalesOrderItemId,
            dto.ProductId,
            dto.ProductName,
            dto.DepartmentId,
            dto.DepartmentName,
            dto.QtyPlanned,
            dto.QtyCut,
            dto.QtySewn,
            dto.QtyPacked,
            dto.QtyFinished,
            dto.Status);
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateProductionOrderResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateProductionOrderRequest), typeof(CreateProductionOrderRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateProductionOrderResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateProductionOrderRequest req)
    {
        var id = await _mediator.Send(new CreateProductionOrderCommand
        {
            SalesOrderItemId = req.SalesOrderItemId,
            DepartmentId = req.DepartmentId,
            QtyPlanned = req.QtyPlanned
        });

        return Created(string.Empty, new CreateProductionOrderResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateProductionOrderRequest), typeof(UpdateProductionOrderRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductionOrderRequest req)
    {
        await _mediator.Send(new UpdateProductionOrderCommand
        {
            ProductionOrderId = id,
            DepartmentId = req.DepartmentId,
            QtyPlanned = req.QtyPlanned
        });

        return NoContent();
    }

    // -------------------------
    //  DELETE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteProductionOrderCommand(id));
        return NoContent();
    }

    // -------------------------
    //  CANCEL
    // -------------------------
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new CancelProductionOrderCommand(id));
        return NoContent();
    }

    // -------------------------
    //  START STAGE
    // -------------------------
    [HttpPost("{id:guid}/start-stage")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(StartProductionStageRequest), typeof(StartProductionStageRequestExample))]
    public async Task<IActionResult> StartStage(Guid id, [FromBody] StartProductionStageRequest req)
    {
        await _mediator.Send(new StartProductionStageCommand(id, req.TargetStatus));
        return NoContent();
    }

    // -------------------------
    //  COMPLETE STAGE
    // -------------------------
    [HttpPost("{id:guid}/complete-stage")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> StartNextStage(Guid id)
    {
        await _mediator.Send(new StartNextProductionStageCommand(id));
        return NoContent();
    }

    // -------------------------
    //  MATERIALS
    // -------------------------
    [HttpGet("{id:guid}/materials")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionOrderMaterialResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderMaterialsResponseExample))]
    public async Task<IActionResult> GetMaterials(Guid id)
    {
        var dtos = await _mediator.Send(new GetProductionOrderMaterialsQuery(id));
        var response = dtos
            .Select(x => new ProductionOrderMaterialResponse(
                x.MaterialId,
                x.MaterialName,
                x.RequiredQty,
                x.AvailableQty,
                x.MissingQty))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  MATERIAL ISSUE DETAILS
    // -------------------------
    [HttpGet("{id:guid}/materials/{materialId:guid}/issue-details")]
    [ProducesResponseType(typeof(ProductionOrderMaterialIssueDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderMaterialIssueDetailsResponseExample))]
    public async Task<IActionResult> GetMaterialIssueDetails(Guid id, Guid materialId)
    {
        var dto = await _mediator.Send(new GetProductionOrderMaterialIssueDetailsQuery
        {
            ProductionOrderId = id,
            MaterialId = materialId
        });

        var response = new ProductionOrderMaterialIssueDetailsResponse(
            new ProductionOrderMaterialResponse(
                dto.Material.MaterialId,
                dto.Material.MaterialName,
                dto.Material.RequiredQty,
                dto.Material.AvailableQty,
                dto.Material.MissingQty),
            dto.Warehouses
                .Select(w => new ProductionOrderMaterialWarehouseResponse(
                    w.WarehouseId,
                    w.WarehouseName,
                    w.AvailableQty))
                .ToList());

        return Ok(response);
    }

    // -------------------------
    //  ISSUE MATERIALS
    // -------------------------
    [HttpPost("{id:guid}/materials/issue")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(IssueMaterialsToProductionRequest), typeof(IssueMaterialsToProductionRequestExample))]
    public async Task<IActionResult> IssueMaterials(Guid id, [FromBody] IssueMaterialsToProductionRequest req)
    {
        await _mediator.Send(new IssueMaterialsToProductionCommand
        {
            ProductionOrderId = id,
            Materials = req.Materials
                .Select(m => new IssueMaterialLineDto
                {
                    MaterialId = m.MaterialId,
                    WarehouseId = m.WarehouseId,
                    Qty = m.Qty
                })
                .ToList()
        });

        return NoContent();
    }

    // -------------------------
    //  STAGES SUMMARY
    // -------------------------
    [HttpGet("{id:guid}/stages")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionStageSummaryResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionStagesResponseExample))]
    public async Task<IActionResult> GetStages(Guid id)
    {
        var dtos = await _mediator.Send(new GetProductionStagesQuery(id));
        var response = dtos?
            .Select(x => new ProductionStageSummaryResponse(x.Stage, x.CompletedQty, x.RemainingQty))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  STAGE EMPLOYEES
    // -------------------------
    [HttpGet("{id:guid}/stages/{stage}")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionStageAssignmentResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionStageAssignmentsResponseExample))]
    public async Task<IActionResult> GetStageEmployees(Guid id, ProductionStage stage)
    {
        var dtos = await _mediator.Send(new GetProductionStageEmployeesQuery(id, stage));

        var response = dtos?
            .Select(x => new ProductionStageAssignmentResponse(
                x.AssignmentId,
                x.Stage,
                x.EmployeeId,
                x.EmployeeName,
                x.PlanPerHour,
                x.AssignedQty,
                x.CompletedQty,
                x.WorkDate
            ))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  AVAILABLE STAGE EMPLOYEES
    // -------------------------
    [HttpGet("{id:guid}/stages/{stage}/available-employees")]
    [ProducesResponseType(typeof(IReadOnlyList<AvailableProductionStageEmployeeResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(AvailableProductionStageEmployeesResponseExample))]
    public async Task<IActionResult> GetAvailableStageEmployees(Guid id, ProductionStage stage)
    {
        var dtos = await _mediator.Send(new GetAvailableProductionStageEmployeesQuery(id, stage));

        var response = dtos
            .Select(x => new AvailableProductionStageEmployeeResponse(
                x.Id,
                x.FullName,
                x.DepartmentName,
                x.PositionName,
                x.IsActive))
            .ToList();

        return Ok(response);
    }

    // -------------------------
    //  ADD STAGE EMPLOYEE
    // -------------------------
    [HttpPost("{id:guid}/stages/{stage}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(AddProductionStageEmployeeRequest), typeof(AddProductionStageEmployeeRequestExample))]
    public async Task<IActionResult> AddStageEmployee(Guid id, ProductionStage stage, [FromBody] AddProductionStageEmployeeRequest req)
    {
        await _mediator.Send(new AddProductionStageEmployeeCommand(
            id,
            stage,
            req.EmployeeId,
            req.AssignedQty,
            req.CompletedQty,
            req.Date));

        return NoContent();
    }

    // -------------------------
    //  UPDATE STAGE EMPLOYEE
    // -------------------------
    [HttpPut("{id:guid}/stages/{stage}/employees/{assignmentId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateProductionStageEmployeeRequest), typeof(UpdateProductionStageEmployeeRequestExample))]
    public async Task<IActionResult> UpdateStageEmployee(
        Guid id,
        ProductionStage stage,
        Guid assignmentId,
        [FromBody] UpdateProductionStageEmployeeRequest req)
    {
        await _mediator.Send(new UpdateProductionStageEmployeeCommand(
            assignmentId,
            stage,
            id,
            req.EmployeeId,
            req.AssignedQty,
            req.CompletedQty,
            req.Date));

        return NoContent();
    }

    // -------------------------
    //  REMOVE STAGE EMPLOYEE
    // -------------------------
    [HttpDelete("{id:guid}/stages/{stage}/employees/{assignmentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveStageEmployee(Guid id, ProductionStage stage, Guid assignmentId)
    {
        await _mediator.Send(new RemoveProductionStageEmployeeCommand(id, assignmentId, stage));
        return NoContent();
    }

    // -------------------------
    //  REGISTER SEWING OPERATION
    // -------------------------
    [HttpPost("{id:guid}/sewing-operations")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RegisterSewingOperationResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(RegisterSewingOperationRequest), typeof(RegisterSewingOperationRequestExample))]
    [SwaggerResponseExample(201, typeof(RegisterSewingOperationResponseExample))]
    public async Task<IActionResult> RegisterSewingOperation(Guid id, [FromBody] RegisterSewingOperationRequest req)
    {
        var operationId = await _mediator.Send(new RegisterSewingOperationCommand(
            id,
            req.AssignmentId,
            req.EmployeeId,
            req.QtySewn,
            req.HoursWorked,
            req.OperationDate));

        return Created(string.Empty, new RegisterSewingOperationResponse(operationId));
    }

    // -------------------------
    //  SHIP FINISHED GOODS
    // -------------------------
    [HttpPost("{id:guid}/ship")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(ShipFinishedGoodsRequest), typeof(ShipFinishedGoodsRequestExample))]
    public async Task<IActionResult> Ship(Guid id, [FromBody] ShipFinishedGoodsRequest req)
    {
        await _mediator.Send(new ShipFinishedGoodsCommand
        {
            ProductionOrderId = id,
            FromWarehouseId = req.FromWarehouseId,
            ToWarehouseId = req.ToWarehouseId,
            QtyPerPackage = req.QtyPerPackage,
            PackageCount = req.PackageCount
        });
        
        return NoContent();
    }

    // -------------------------
    //  SHIPMENTS
    // -------------------------
    [HttpGet("{id:guid}/shipments")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionOrderShipmentResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderShipmentsResponseExample))]
    public async Task<IActionResult> GetShipments(Guid id)
    {
        var dtos = await _mediator.Send(new GetProductionOrderShipmentsQuery(id));
        var response = dtos
            .Select(x => new ProductionOrderShipmentResponse(
                x.WarehouseId,
                x.WarehouseName,
                x.Qty,
                x.ShipmentDate))
            .ToList();
        return Ok(response);
    }
}
