using System.Net.Http.Json;
using MyFactory.MauiClient.Models.MaterialPurchaseOrders;

namespace MyFactory.MauiClient.Services.MaterialPurchaseOrders;

public sealed class MaterialPurchaseOrdersService : IMaterialPurchaseOrdersService
{
    private readonly HttpClient _httpClient;

    public MaterialPurchaseOrdersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CreateMaterialPurchaseOrderResponse?> CreateAsync(CreateMaterialPurchaseOrderRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/material-purchase-orders", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateMaterialPurchaseOrderResponse>();
    }

    public async Task AddItemAsync(Guid purchaseOrderId, AddMaterialPurchaseOrderItemRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/material-purchase-orders/{purchaseOrderId}/items", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task ConfirmAsync(Guid purchaseOrderId)
    {
        var response = await _httpClient.PostAsync($"api/material-purchase-orders/{purchaseOrderId}/confirm", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task ReceiveAsync(Guid purchaseOrderId, ReceiveMaterialPurchaseOrderRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/material-purchase-orders/{purchaseOrderId}/receive", request);
        response.EnsureSuccessStatusCode();
    }
}
