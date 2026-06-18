using System.Net.Http.Json;
using MyFactory.MauiClient.Models.MaterialTypes;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.MaterialTypes;

public sealed class MaterialTypesService : IMaterialTypesService
{
    private readonly HttpClient _httpClient;

    public MaterialTypesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private const string BaseRoute = "api/materialtypes";

    public async Task<IReadOnlyList<MaterialTypeModel>> GetListAsync(bool usedOnly = false, CancellationToken ct = default)
    {
        var query = new List<string>
        {
            $"usedOnly={usedOnly.ToString().ToLowerInvariant()}"
        };

        var path = BaseRoute + $"?{string.Join("&", query)}";

        var result = await _httpClient
            .GetFromJsonAsync<IReadOnlyList<MaterialTypeModel>>(path, ct);

        return result ?? [];
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
        await response.EnsureSuccessWithProblemAsync(ct);

        var body = await response.Content
            .ReadFromJsonAsync<CreateMaterialTypeResponse>(cancellationToken: ct);

        return body?.Id
            ?? throw new InvalidOperationException("Invalid create response");
    }

    public async Task UpdateAsync(Guid id, UpdateMaterialTypeRequest request, CancellationToken ct = default)
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