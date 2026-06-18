using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.ProductionOrders;

public interface IProductionOrdersService : IGetListService<ProductionOrderListItemResponse>
{
    Task<ListResponse<ProductionOrderListItemResponse>?> GetListByDateAsync(
        Guid? searchSaleOrderId = null,
        string? searchProductionOrderNumber = null,
        string? searchSaleOrderNumber = null,
        string? searchCustomerName = null,
        string? searchProductName = null,
        string? sortBy = null,
        bool sortDesc = false,
        int skip = 0,
        int take = 30,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        ProductionOrderStatus? status = null);
    Task<IReadOnlyList<ProductionOrderListItemResponse>?> GetBySalesOrderAsync(Guid salesOrderId);
    Task<ProductionOrderDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CreateProductionOrderResponse?> CreateAsync(CreateProductionOrderRequest request);
    Task UpdateAsync(Guid id, UpdateProductionOrderRequest request);
    Task DeleteAsync(Guid id);
    Task CancelAsync(Guid id);
    Task StartStageAsync(Guid id, StartProductionStageRequest request);
    Task StartNextStageAsync(Guid id);
    Task<IReadOnlyList<ProductionOrderMaterialResponse>?> GetMaterialsAsync(Guid id);
    Task<ProductionOrderMaterialIssueDetailsResponse?> GetMaterialIssueDetailsAsync(Guid id, Guid materialId);
    Task IssueMaterialsAsync(Guid id, IssueMaterialsToProductionRequest request);
    Task<IReadOnlyList<ProductionStageSummaryResponse>?> GetStagesAsync(Guid id);
    Task<IReadOnlyList<ProductionStageAssignmentResponse>?> GetStageEmployeesAsync(Guid id, ProductionStage stage);
    Task<IReadOnlyList<AvailableProductionStageEmployeeResponse>?> GetAvailableStageEmployeesAsync(Guid id, ProductionStage stage);
    Task AddStageEmployeeAsync(Guid id, ProductionStage stage, AddProductionStageEmployeeRequest request);
    Task UpdateStageEmployeeAsync(Guid id, ProductionStage stage, Guid assignmentId, UpdateProductionStageEmployeeRequest request);
    Task RemoveStageEmployeeAsync(Guid id, ProductionStage stage, Guid assignmentId);
    Task<RegisterSewingOperationResponse?> RegisterSewingOperationAsync(Guid id, RegisterSewingOperationRequest request);
    Task ShipAsync(Guid id, ShipFinishedGoodsRequest request);
    Task<IReadOnlyList<ProductionOrderShipmentResponse>?> GetShipmentsAsync(Guid id);
}
