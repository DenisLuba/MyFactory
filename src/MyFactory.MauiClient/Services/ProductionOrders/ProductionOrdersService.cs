using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Services.Common;
using System.Net.Http.Json;

namespace MyFactory.MauiClient.Services.ProductionOrders;

public sealed class ProductionOrdersService : IProductionOrdersService
{
    private readonly HttpClient _httpClient;

    public ProductionOrdersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ListResponse<ProductionOrderListItemResponse>?> GetListByDateAsync(
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
        ProductionOrderStatus? status = null)
    {
        var query = new List<string>();

        if (searchSaleOrderId.HasValue && searchSaleOrderId.Value != Guid.Empty)
            query.Add($"searchSaleOrderId={searchSaleOrderId}");

        if (!string.IsNullOrWhiteSpace(searchProductionOrderNumber))
            query.Add($"searchProductionOrderNumber={searchProductionOrderNumber}");

        if (!string.IsNullOrWhiteSpace(searchSaleOrderNumber))
            query.Add($"searchSaleOrderNumber={searchSaleOrderNumber}");

        if (!string.IsNullOrWhiteSpace(searchCustomerName))
            query.Add($"searchCustomerName={Uri.EscapeDataString(searchCustomerName)}");

        if (!string.IsNullOrWhiteSpace(searchProductName))
            query.Add($"searchProductName={Uri.EscapeDataString(searchProductName)}");

        if (!string.IsNullOrWhiteSpace(sortBy))
            query.Add($"sortBy={Uri.EscapeDataString(sortBy)}");

        if (sortDesc)
            query.Add("sortDesc=true");

        query.Add($"skip={skip}");

        query.Add($"take={take}");

        if (fromDate is not null)
            query.Add($"fromDate={fromDate:yyyy-MM-dd}");

        if (toDate is not null)
            query.Add($"toDate={toDate:yyyy-MM-dd}");

        if (status.HasValue && status.Value is ProductionOrderStatus s)
            query.Add($"status={s}");

        var path = "api/production-orders" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        var response = await _httpClient.GetFromJsonAsync<ListResponse<ProductionOrderListItemResponse>>(path);

        if (response is null) return null;

        var mapped = response.Items
            .Select(x =>
            {
                return new ProductionOrderListItemResponse(
                    x.Id,
                    x.CustomerName,
                    x.ProductionOrderNumber,
                    x.SalesOrderNumber,
                    x.ProductName,
                    x.QtyPlanned,
                    x.QtyFinished,
                    x.Status);
            })
            .ToList();

        return new ListResponse<ProductionOrderListItemResponse>(
            Items: mapped,
            TotalCount: response.TotalCount,
            Take: response.Take,
            Skip: response.Skip,
            HasMore: response.HasMore);
    }

    public async Task<ListResponse<ProductionOrderListItemResponse>?> GetListAsync(
    string? searchName = null,
    string? searchType = null,
    string? sortBy = null,
    bool sortDesc = false,
    int skip = 0,
    int take = 30,
    bool? isActive = null,
    Guid? fromId = null)
    {
        return await GetListByDateAsync(
            searchProductionOrderNumber: searchName,
            sortBy: sortBy,
            sortDesc: sortDesc,
            skip: skip,
            take: take);
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

    public async Task StartNextStageAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/production-orders/{id}/complete-stage", null);
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

    public async Task<IReadOnlyList<ProductionStageAssignmentResponse>?> GetStageEmployeesAsync(Guid id, ProductionStage stage)
    {
        return await _httpClient.GetFromJsonAsync<List<ProductionStageAssignmentResponse>>($"api/production-orders/{id}/stages/{stage}");
    }

    public async Task<IReadOnlyList<AvailableProductionStageEmployeeResponse>?> GetAvailableStageEmployeesAsync(Guid id, ProductionStage stage)
    {
        return await _httpClient.GetFromJsonAsync<List<AvailableProductionStageEmployeeResponse>>($"api/production-orders/{id}/stages/{stage}/available-employees");
    }

    public async Task AddStageEmployeeAsync(Guid id, ProductionStage stage, AddProductionStageEmployeeRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/production-orders/{id}/stages/{stage}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task UpdateStageEmployeeAsync(Guid id, ProductionStage stage, Guid assignmentId, UpdateProductionStageEmployeeRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/production-orders/{id}/stages/{stage}/employees/{assignmentId}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task RemoveStageEmployeeAsync(Guid id, ProductionStage stage, Guid assignmentId)
    {
        var response = await _httpClient.DeleteAsync($"api/production-orders/{id}/stages/{stage}/employees/{assignmentId}");
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task<RegisterSewingOperationResponse?> RegisterSewingOperationAsync(Guid id, RegisterSewingOperationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/production-orders/{id}/sewing-operations", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<RegisterSewingOperationResponse>();
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
