using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Services.Common;
using System.Net.Http.Json;

namespace MyFactory.MauiClient.Services.SalesOrders;

public sealed class SalesOrdersService : ISalesOrdersService
{
    private readonly HttpClient _httpClient;

    public SalesOrdersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ListResponse<SalesOrderListItemResponse>?> GetListByDateAsync(
        string? searchName = null,
        string? sortBy = null,
        bool sortDesc = false,
        int skip = 0,
        int take = 30,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        SalesOrderStatus? status = null)
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(searchName))
            query.Add($"searchName={Uri.EscapeDataString(searchName)}");

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

        if (status.HasValue && status.Value is SalesOrderStatus s)
            query.Add($"status={s}");

        var path = "api/sales-orders" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        var response = await _httpClient.GetFromJsonAsync<ListResponse<SalesOrderListItemResponse>>(path);

        if (response is null) return null;

        var mapped = response.Items
            .Select(x =>
            {
                var utc = DateTime.SpecifyKind(x.OrderDate, DateTimeKind.Utc);
                var local = utc.ToLocalTime();
                return new SalesOrderListItemResponse(
                    x.Id,
                    x.OrderNumber,
                    x.CustomerName,
                    local,
                    x.Status);
            })
            .ToList();

        return new ListResponse<SalesOrderListItemResponse>(
            Items: mapped,
            TotalCount: response.TotalCount,
            Take: response.Take,
            Skip: response.Skip,
            HasMore: response.HasMore);
    }

    public async Task<ListResponse<SalesOrderListItemResponse>?> GetListAsync(
        string? searchName =null,
        string? searchType = null,
        string? sortBy = null, 
        bool sortDesc = false,
        int skip = 0,
        int take = 30,
        bool? isActive = null,
        Guid? fromId = null)
    {
        return await GetListByDateAsync(
            searchName, 
            sortBy, 
            sortDesc, 
            skip, 
            take, 
            fromDate: null, 
            toDate: null);
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
