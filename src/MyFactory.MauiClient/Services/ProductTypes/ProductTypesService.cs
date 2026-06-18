using System.Net.Http.Json;
using MyFactory.MauiClient.Models.ProductTypes;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.ProductTypes;

public sealed class ProductTypesService : IProductTypesService
{
    private readonly HttpClient _httpClient;
    private const string BaseRoute = "api/producttypes";

    public ProductTypesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ProductTypeModel>> GetListAsync(string? search = null, CancellationToken ct = default)
    {
        var query = string.IsNullOrWhiteSpace(search)
            ? string.Empty
            : $"?search={Uri.EscapeDataString(search)}";

        var result = await _httpClient
            .GetFromJsonAsync<IReadOnlyList<ProductTypeModel>>($"{BaseRoute}{query}", ct);

        return result ?? Array.Empty<ProductTypeModel>();
    }

    public async Task<ProductTypeModel> GetDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var result = await _httpClient
            .GetFromJsonAsync<ProductTypeModel>($"{BaseRoute}/{id}", ct);

        return result ?? throw new InvalidOperationException("Product type not found");
    }

    public async Task<ProductTypeModel?> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/by-product/{productId}", ct);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        await response.EnsureSuccessWithProblemAsync(ct);

        return await response.Content.ReadFromJsonAsync<ProductTypeModel>(cancellationToken: ct);
    }

    public async Task<Guid> CreateAsync(CreateProductTypeRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, ct);
        await response.EnsureSuccessWithProblemAsync(ct);

        var body = await response.Content.ReadFromJsonAsync<CreateProductTypeResponse>(cancellationToken: ct);
        return body?.Id ?? throw new InvalidOperationException("Invalid create response");
    }

    public async Task UpdateAsync(Guid id, UpdateProductTypeRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}", request, ct);
        await response.EnsureSuccessWithProblemAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _httpClient.DeleteAsync($"{BaseRoute}/{id}", ct);
        await response.EnsureSuccessWithProblemAsync(ct);
    }
}
