using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Products;

namespace MyFactory.MauiClient.Services.ProductsServices;

public class ProductsService(HttpClient httpClient) : IProductsService
{
    private readonly HttpClient _httpClient = httpClient;

    public Task<IReadOnlyList<ProductsListResponse>?> GetProductsAsync()
        => _httpClient.GetFromJsonAsync<IReadOnlyList<ProductsListResponse>>("api/products");

    public Task<ProductCardResponse?> GetProductAsync(Guid id)
        => _httpClient.GetFromJsonAsync<ProductCardResponse>($"api/products/{id}");

    public async Task<ProductUpdateResponse?> UpdateProductAsync(Guid id, ProductUpdateRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", request);
        return await response.Content.ReadFromJsonAsync<ProductUpdateResponse>();
    }

    public Task<IReadOnlyList<ProductBomItemResponse>?> GetBomAsync(Guid productId)
        => _httpClient.GetFromJsonAsync<IReadOnlyList<ProductBomItemResponse>>($"api/products/{productId}/bom");

    public Task<IReadOnlyList<ProductOperationItemResponse>?> GetOperationsAsync(Guid productId)
        => _httpClient.GetFromJsonAsync<IReadOnlyList<ProductOperationItemResponse>>($"api/products/{productId}/operations");

    public async Task<ProductBomItemResponse?> AddBomItemAsync(Guid productId, ProductBomCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/products/{productId}/bom", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProductBomItemResponse>();
    }

    public async Task<ProductBomDeleteResponse?> DeleteBomItemAsync(Guid productId, Guid lineId)
    {
        var response = await _httpClient.DeleteAsync($"api/products/{productId}/bom/{lineId}");
        return await response.Content.ReadFromJsonAsync<ProductBomDeleteResponse>();
    }

    public async Task<ProductOperationItemResponse?> AddOperationAsync(Guid productId, ProductOperationCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/products/{productId}/operations", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProductOperationItemResponse>();
    }

    public async Task<ProductOperationDeleteResponse?> DeleteOperationAsync(Guid productId, Guid lineId)
    {
        var response = await _httpClient.DeleteAsync($"api/products/{productId}/operations/{lineId}");
        return await response.Content.ReadFromJsonAsync<ProductOperationDeleteResponse>();
    }
}
