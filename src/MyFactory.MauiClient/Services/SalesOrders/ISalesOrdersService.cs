using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.SalesOrders;

public interface ISalesOrdersService : IGetListService<SalesOrderListItemResponse>
{
    Task<ListResponse<SalesOrderListItemResponse>?> GetListByDateAsync(
        string? searchName = null,
        string? sortBy = null,
        bool sortDesc = false,
        int skip = 0,
        int take = 30,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        SalesOrderStatus? status = null);
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
