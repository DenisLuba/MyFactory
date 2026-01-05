using MyFactory.MauiClient.Models.ProductionOrders;

namespace MyFactory.MauiClient.Services.ProductionOrders;

public interface IProductionOrdersService
{
    Task<IReadOnlyList<ProductionOrderListItemResponse>?> GetListAsync();
    Task<IReadOnlyList<ProductionOrderListItemResponse>?> GetBySalesOrderAsync(Guid salesOrderId);
    Task<ProductionOrderDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CreateProductionOrderResponse?> CreateAsync(CreateProductionOrderRequest request);
    Task UpdateAsync(Guid id, UpdateProductionOrderRequest request);
    Task DeleteAsync(Guid id);
    Task CancelAsync(Guid id);
    Task StartStageAsync(Guid id, StartProductionStageRequest request);
    Task CompleteStageAsync(Guid id, CompleteProductionStageRequest request);
    Task<IReadOnlyList<ProductionOrderMaterialResponse>?> GetMaterialsAsync(Guid id);
    Task<ProductionOrderMaterialIssueDetailsResponse?> GetMaterialIssueDetailsAsync(Guid id, Guid materialId);
    Task IssueMaterialsAsync(Guid id, IssueMaterialsToProductionRequest request);
    Task<IReadOnlyList<ProductionStageSummaryResponse>?> GetStagesAsync(Guid id);
    Task<IReadOnlyList<ProductionStageEmployeeResponse>?> GetStageEmployeesAsync(Guid id, ProductionOrderStatus stage);
    Task AddStageEmployeeAsync(Guid id, ProductionOrderStatus stage, AddProductionStageEmployeeRequest request);
    Task UpdateStageEmployeeAsync(Guid id, ProductionOrderStatus stage, Guid operationId, UpdateProductionStageEmployeeRequest request);
    Task RemoveStageEmployeeAsync(Guid id, ProductionOrderStatus stage, Guid operationId);
    Task ShipAsync(Guid id, ShipFinishedGoodsRequest request);
    Task<IReadOnlyList<ProductionOrderShipmentResponse>?> GetShipmentsAsync(Guid id);
}
