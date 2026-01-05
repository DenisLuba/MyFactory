using MyFactory.MauiClient.Models.SalesOrders;

namespace MyFactory.MauiClient.Services.SalesOrders;

public interface ISalesOrdersService
{
    Task<IReadOnlyList<SalesOrderListItemResponse>?> GetListAsync();
    Task<SalesOrderDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CreateSalesOrderResponse?> CreateAsync(CreateSalesOrderRequest request);
    Task UpdateAsync(Guid id, UpdateSalesOrderRequest request);
    Task StartAsync(Guid id);
    Task CompleteAsync(Guid id);
    Task CancelAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<AddSalesOrderItemResponse?> AddItemAsync(Guid salesOrderId, AddSalesOrderItemRequest request);
    Task UpdateItemAsync(Guid itemId, UpdateSalesOrderItemRequest request);
    Task RemoveItemAsync(Guid itemId);
    Task<IReadOnlyList<SalesOrderShipmentResponse>?> GetShipmentsAsync(Guid salesOrderId);
}
