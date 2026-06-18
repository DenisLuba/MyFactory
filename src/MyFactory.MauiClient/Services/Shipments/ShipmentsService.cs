using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Shipments;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Shipments;

internal sealed class ShipmentsService : IShipmentsService
{
    private readonly HttpClient _httpClient;

    public ShipmentsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ShipmentListItemResponse>?> GetListAsync(
        Guid? salesOrderId = null,
        Guid? customerId = null,
        ShipmentStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken ct = default)
    {
        var query = new List<string>();
        if (salesOrderId is Guid soId)
            query.Add($"salesOrderId={soId}");
        if (customerId is Guid cId)
            query.Add($"customerId={cId}");
        if (status is not null)
            query.Add($"status={MapToApiStatus(status.Value)}"); // status mapping remains string output
        if (fromDate is not null)
            query.Add($"fromDate={fromDate:O}");
        if (toDate is not null)
            query.Add($"toDate={toDate:O}");

        var path = "/api/shipments" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        return await _httpClient.GetFromJsonAsync<List<ShipmentListItemResponse>>(path, ct);
    }

    public async Task<ShipmentDetailsResponse?> GetDetailsAsync(Guid id, CancellationToken ct = default)
    {
        return await _httpClient.GetFromJsonAsync<ShipmentDetailsResponse>($"/api/shipments/{id}", ct);
    }

    public async Task<Guid> CreateAsync(CreateShipmentRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/shipments", request, ct);
        await response.EnsureSuccessWithProblemAsync(ct);

        var body = await response.Content.ReadFromJsonAsync<CreateShipmentResponse>(cancellationToken: ct);
        return body?.Id ?? throw new InvalidOperationException("Invalid create shipment response");
    }

    public async Task UpdateAsync(Guid id, UpdateShipmentRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/shipments/{id}", request, ct);
        await response.EnsureSuccessWithProblemAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _httpClient.DeleteAsync($"/api/shipments/{id}", ct);
        await response.EnsureSuccessWithProblemAsync(ct);
    }

    private static string MapToApiStatus(ShipmentStatus status) => status.ToString();
    public static ShipmentStatus MapFromApiStatus(string status) => Enum.Parse<ShipmentStatus>(status, ignoreCase: true);
}
