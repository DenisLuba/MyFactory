using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Production.MaterialTransfers;
using MyFactory.MauiClient.Models.Production.ProductionOrders;

namespace MyFactory.MauiClient.Services.ProductionServices;

public class ProductionOrdersService(HttpClient httpClient) : IProductionOrdersService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IReadOnlyList<ProductionOrderListResponse>?> GetListAsync()
    {
        IReadOnlyList<ProductionOrderListResponse>? orders = await _httpClient.GetFromJsonAsync<List<ProductionOrderListResponse>>("api/production-orders");
        return orders ?? Array.Empty<ProductionOrderListResponse>();
    }

    public async Task<ProductionOrderCardResponse?> GetByIdAsync(Guid orderId)
        => await _httpClient.GetFromJsonAsync<ProductionOrderCardResponse>($"api/production-orders/{orderId}");

    public async Task<IReadOnlyList<MaterialTransferItemDto>?> GetMaterialTransfersAsync(Guid orderId)
    {
        IReadOnlyList<MaterialTransferItemDto>? transfers = await _httpClient.GetFromJsonAsync<List<MaterialTransferItemDto>>($"api/production-orders/{orderId}/material-transfers");
        return transfers ?? Array.Empty<MaterialTransferItemDto>();
    }

    public async Task<IReadOnlyList<StageDistributionItem>?> GetStageDistributionAsync(Guid orderId)
    {
        IReadOnlyList<StageDistributionItem>? stages = await _httpClient.GetFromJsonAsync<List<StageDistributionItem>>($"api/production-orders/{orderId}/stage-distribution");
        return stages ?? Array.Empty<StageDistributionItem>();
    }
}
