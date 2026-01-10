using System.Net.Http.Json;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.SalesOrders;

public sealed class SalesOrdersService : ISalesOrdersService
{
    private readonly HttpClient _httpClient;

    public SalesOrdersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<SalesOrderListItemResponse>?> GetListAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<SalesOrderListItemResponse>>("api/sales-orders");
    }

    public async Task<SalesOrderDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<SalesOrderDetailsResponse>($"api/sales-orders/{id}");
    }

    public async Task<CreateSalesOrderResponse?> CreateAsync(CreateSalesOrderRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/sales-orders", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateSalesOrderResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateSalesOrderRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/sales-orders/{id}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task StartAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/sales-orders/{id}/start", null);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task CompleteAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/sales-orders/{id}/complete", null);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task CancelAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/sales-orders/{id}/cancel", null);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/sales-orders/{id}");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task<AddSalesOrderItemResponse?> AddItemAsync(Guid salesOrderId, AddSalesOrderItemRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/sales-orders/{salesOrderId}/items", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<AddSalesOrderItemResponse>();
    }

    public async Task UpdateItemAsync(Guid itemId, UpdateSalesOrderItemRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/sales-orders/items/{itemId}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task RemoveItemAsync(Guid itemId)
    {
        var response = await _httpClient.DeleteAsync($"api/sales-orders/items/{itemId}");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task<IReadOnlyList<SalesOrderShipmentResponse>?> GetShipmentsAsync(Guid salesOrderId)
    {
        return await _httpClient.GetFromJsonAsync<List<SalesOrderShipmentResponse>>($"api/sales-orders/{salesOrderId}/shipments");
    }
}
