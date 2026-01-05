using System.Net.Http.Headers;
using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Products;

namespace MyFactory.MauiClient.Services.Products;

public sealed class ProductsService : IProductsService
{
    private readonly HttpClient _httpClient;

    public ProductsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ProductListItemResponse>?> GetListAsync(string? search = null, string? sortBy = null, bool sortDesc = false)
    {
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(search)) query.Add($"search={Uri.EscapeDataString(search)}");
        if (!string.IsNullOrWhiteSpace(sortBy)) query.Add($"sortBy={Uri.EscapeDataString(sortBy)}");
        if (sortDesc) query.Add("sortDesc=true");

        var path = "api/products" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        return await _httpClient.GetFromJsonAsync<List<ProductListItemResponse>>(path);
    }

    public async Task<ProductDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<ProductDetailsResponse>($"api/products/{id}");
    }

    public async Task<CreateProductResponse?> CreateAsync(CreateProductRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/products", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateProductResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<AddProductMaterialResponse?> AddMaterialAsync(Guid productId, AddProductMaterialRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/products/{productId}/materials", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AddProductMaterialResponse>();
    }

    public async Task UpdateMaterialAsync(Guid productMaterialId, UpdateProductMaterialRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/products/materials/{productMaterialId}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveMaterialAsync(Guid productMaterialId)
    {
        var response = await _httpClient.DeleteAsync($"api/products/materials/{productMaterialId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task SetProductionCostsAsync(Guid productId, SetProductProductionCostsRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/products/{productId}/production-costs", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IReadOnlyList<ProductImageFileResponse>?> GetImagesAsync(Guid productId)
    {
        return await _httpClient.GetFromJsonAsync<List<ProductImageFileResponse>>($"api/products/{productId}/images");
    }

    public async Task<byte[]?> GetImageContentAsync(Guid imageId, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync($"api/products/images/{imageId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    public async Task<Guid?> UploadImageAsync(Guid productId, Stream content, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        using var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
        using var form = new MultipartFormDataContent
        {
            { streamContent, "file", fileName }
        };

        using var response = await _httpClient.PostAsync($"api/products/{productId}/images", form, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>(cancellationToken: cancellationToken);
    }

    public async Task DeleteImageAsync(Guid imageId)
    {
        var response = await _httpClient.DeleteAsync($"api/products/images/{imageId}");
        response.EnsureSuccessStatusCode();
    }
}
