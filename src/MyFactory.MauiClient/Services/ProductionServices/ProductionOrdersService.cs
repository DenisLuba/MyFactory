using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Production.MaterialTransfers;
using MyFactory.MauiClient.Models.Production.ProductionOrders;

namespace MyFactory.MauiClient.Services.ProductionServices;

public class ProductionOrdersService(HttpClient httpClient) : IProductionOrdersService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IReadOnlyList<ProductionOrderListResponse>?> GetListAsync()
        => await _httpClient.GetFromJsonAsync<IReadOnlyList<ProductionOrderListResponse>>("api/production-orders");

    public async Task<ProductionOrderCardResponse?> GetByIdAsync(Guid orderId)
        => await _httpClient.GetFromJsonAsync<ProductionOrderCardResponse>($"api/production-orders/{orderId}");

    public async Task<IReadOnlyList<MaterialTransferItemDto>?> GetMaterialTransfersAsync(Guid orderId)
        => await _httpClient.GetFromJsonAsync<IReadOnlyList<MaterialTransferItemDto>>($"api/production-orders/{orderId}/material-transfers");

    public async Task<IReadOnlyList<StageDistributionItem>?> GetStageDistributionAsync(Guid orderId)
        => await _httpClient.GetFromJsonAsync<IReadOnlyList<StageDistributionItem>>($"api/production-orders/{orderId}/stage-distribution");
}
