using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Shipments;

namespace MyFactory.MauiClient.Services.ShipmentsServices;

public class ShipmentsService(HttpClient httpClient) : IShipmentsService
{
    private readonly HttpClient _httpClient = httpClient;

    /*public async Task<ShipmentsCreateResponse?> CreateShipmentAsync(ShipmentsCreateRequest request)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/shipments", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ShipmentsCreateResponse>();
    }*/

    public async Task<List<ShipmentsListResponse>?> GetShipmentsAsync()
        => await _httpClient.GetFromJsonAsync<List<ShipmentsListResponse>>("api/shipments");

    public async Task<ShipmentCardResponse?> GetShipmentByIdAsync(Guid shipmentId)
        => await _httpClient.GetFromJsonAsync<ShipmentCardResponse>($"api/shipments/{shipmentId}");

    public async Task<ShipmentsConfirmPaymentResponse?> ConfirmPaymentAsync(Guid shipmentId)
    {
        using var response = await _httpClient.PostAsync($"api/shipments/{shipmentId}/confirm-payment", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ShipmentsConfirmPaymentResponse>();
    }
}