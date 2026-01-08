using System.Net.Http.Json;
using MyFactory.MauiClient.Models.MaterialTypes;

namespace MyFactory.MauiClient.Services.MaterialTypes;

public sealed class MaterialTypesService : IMaterialTypesService
{
    private readonly HttpClient _httpClient;

    public MaterialTypesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private const string BaseRoute = "api/materialtypes";

    public async Task<IReadOnlyList<MaterialTypeModel>> GetListAsync(CancellationToken ct = default)
    {
        var result = await _httpClient
            .GetFromJsonAsync<IReadOnlyList<MaterialTypeModel>>(BaseRoute, ct);

        return result ?? Array.Empty<MaterialTypeModel>();
    }

    public async Task<MaterialTypeModel> GetDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var result = await _httpClient
            .GetFromJsonAsync<MaterialTypeModel>($"{BaseRoute}/{id}", ct);

        return result
            ?? throw new InvalidOperationException("MaterialType not found");
    }

    public async Task<Guid> CreateAsync(CreateMaterialTypeRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, ct);
        response.EnsureSuccessStatusCode();

        var body = await response.Content
            .ReadFromJsonAsync<CreateMaterialTypeResponse>(cancellationToken: ct);

        return body?.Id
            ?? throw new InvalidOperationException("Invalid create response");
    }

    public async Task UpdateAsync(Guid id, UpdateMaterialTypeRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}", request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _httpClient.DeleteAsync($"{BaseRoute}/{id}", ct);
        response.EnsureSuccessStatusCode();
    }
}