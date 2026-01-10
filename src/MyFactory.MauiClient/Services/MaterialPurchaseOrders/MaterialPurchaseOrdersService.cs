using System.Net.Http.Json;
using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Common;

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
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateMaterialPurchaseOrderResponse>();
    }

    public async Task AddItemAsync(Guid purchaseOrderId, AddMaterialPurchaseOrderItemRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/material-purchase-orders/{purchaseOrderId}/items", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task ConfirmAsync(Guid purchaseOrderId)
    {
        var response = await _httpClient.PostAsync($"api/material-purchase-orders/{purchaseOrderId}/confirm", null);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task ReceiveAsync(Guid purchaseOrderId, ReceiveMaterialPurchaseOrderRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/material-purchase-orders/{purchaseOrderId}/receive", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task<IReadOnlyList<SupplierPurchaseOrderListItemResponse>?> GetBySupplierAsync(Guid supplierId)
    {
        return await _httpClient.GetFromJsonAsync<IReadOnlyList<SupplierPurchaseOrderListItemResponse>>(
            $"api/material-purchase-orders/supplier/{supplierId}");
    }

    public async Task<MaterialPurchaseOrderDetailsResponse?> GetDetailsAsync(Guid purchaseOrderId)
    {
        return await _httpClient.GetFromJsonAsync<MaterialPurchaseOrderDetailsResponse>(
            $"api/material-purchase-orders/{purchaseOrderId}");
    }

    public async Task UpdateItemAsync(Guid itemId, UpdateMaterialPurchaseOrderItemRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/material-purchase-orders/items/{itemId}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task RemoveItemAsync(Guid itemId)
    {
        var response = await _httpClient.DeleteAsync($"api/material-purchase-orders/items/{itemId}");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task CancelAsync(Guid purchaseOrderId)
    {
        var response = await _httpClient.PostAsync($"api/material-purchase-orders/{purchaseOrderId}/cancel", null);
        await response.EnsureSuccessWithProblemAsync();
    }
}
