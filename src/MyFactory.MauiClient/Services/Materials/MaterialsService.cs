using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Materials;

public sealed class MaterialsService : IMaterialsService
{
    private readonly HttpClient _httpClient;

    public MaterialsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<MaterialListItemResponse>?> GetListAsync(string? materialName = null, string? materialType = null, bool? isActive = null, Guid? warehouseId = null)
    {
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(materialName))
            query.Add($"materialName={Uri.EscapeDataString(materialName)}");
        if (!string.IsNullOrWhiteSpace(materialType))
            query.Add($"materialType={Uri.EscapeDataString(materialType)}");
        if (isActive.HasValue)
            query.Add($"isActive={isActive.Value.ToString().ToLowerInvariant()}");
        if (warehouseId.HasValue)
            query.Add($"warehouseId={warehouseId.Value}");

        var path = "/api/materials" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        return await _httpClient.GetFromJsonAsync<List<MaterialListItemResponse>>(path);
    }

    public async Task<MaterialDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<MaterialDetailsResponse>($"/api/materials/{id}");
    }

    public async Task<Guid> CreateAsync(CreateMaterialRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/materials", request, ct);
        await response.EnsureSuccessWithProblemAsync(ct);

        var body = await response.Content.ReadFromJsonAsync<CreateMaterialResponse>(cancellationToken: ct);
        return body?.Id ?? throw new InvalidOperationException("Invalid create material response");
    }

    public async Task UpdateAsync(Guid id, UpdateMaterialRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/materials/{id}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _httpClient.DeleteAsync($"/api/materials/{id}", ct);
        await response.EnsureSuccessWithProblemAsync(ct);
    }
}
