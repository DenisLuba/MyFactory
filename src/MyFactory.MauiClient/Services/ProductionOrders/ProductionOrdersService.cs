using System.Net.Http.Json;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.ProductionOrders;

public sealed class ProductionOrdersService : IProductionOrdersService
{
    private readonly HttpClient _httpClient;

    public ProductionOrdersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ProductionOrderListItemResponse>?> GetListAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProductionOrderListItemResponse>>("api/production-orders");
    }

    public async Task<IReadOnlyList<ProductionOrderListItemResponse>?> GetBySalesOrderAsync(Guid salesOrderId)
    {
        return await _httpClient.GetFromJsonAsync<List<ProductionOrderListItemResponse>>($"api/production-orders/sales-order/{salesOrderId}");
    }

    public async Task<ProductionOrderDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<ProductionOrderDetailsResponse>($"api/production-orders/{id}");
    }

    public async Task<CreateProductionOrderResponse?> CreateAsync(CreateProductionOrderRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/production-orders", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreateProductionOrderResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateProductionOrderRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/production-orders/{id}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/production-orders/{id}");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task CancelAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/production-orders/{id}/cancel", null);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task StartStageAsync(Guid id, StartProductionStageRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/production-orders/{id}/start-stage", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task CompleteStageAsync(Guid id, CompleteProductionStageRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/production-orders/{id}/complete-stage", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task<IReadOnlyList<ProductionOrderMaterialResponse>?> GetMaterialsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<List<ProductionOrderMaterialResponse>>($"api/production-orders/{id}/materials");
    }

    public async Task<ProductionOrderMaterialIssueDetailsResponse?> GetMaterialIssueDetailsAsync(Guid id, Guid materialId)
    {
        return await _httpClient.GetFromJsonAsync<ProductionOrderMaterialIssueDetailsResponse>($"api/production-orders/{id}/materials/{materialId}/issue-details");
    }

    public async Task IssueMaterialsAsync(Guid id, IssueMaterialsToProductionRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/production-orders/{id}/materials/issue", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task<IReadOnlyList<ProductionStageSummaryResponse>?> GetStagesAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<List<ProductionStageSummaryResponse>>($"api/production-orders/{id}/stages");
    }

    public async Task<IReadOnlyList<ProductionStageEmployeeResponse>?> GetStageEmployeesAsync(Guid id, ProductionOrderStatus stage)
    {
        return await _httpClient.GetFromJsonAsync<List<ProductionStageEmployeeResponse>>($"api/production-orders/{id}/stages/{stage}");
    }

    public async Task AddStageEmployeeAsync(Guid id, ProductionOrderStatus stage, AddProductionStageEmployeeRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/production-orders/{id}/stages/{stage}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task UpdateStageEmployeeAsync(Guid id, ProductionOrderStatus stage, Guid operationId, UpdateProductionStageEmployeeRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/production-orders/{id}/stages/{stage}/employees/{operationId}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task RemoveStageEmployeeAsync(Guid id, ProductionOrderStatus stage, Guid operationId)
    {
        var response = await _httpClient.DeleteAsync($"api/production-orders/{id}/stages/{stage}/employees/{operationId}");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task ShipAsync(Guid id, ShipFinishedGoodsRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/production-orders/{id}/ship", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task<IReadOnlyList<ProductionOrderShipmentResponse>?> GetShipmentsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<List<ProductionOrderShipmentResponse>>($"api/production-orders/{id}/shipments");
    }
}
