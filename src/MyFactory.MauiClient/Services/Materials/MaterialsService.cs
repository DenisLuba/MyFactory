using System.Net.Http.Json;
using System.Web;
using MyFactory.MauiClient.Models.Materials;

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
        var builder = new UriBuilder(new Uri(_httpClient.BaseAddress!, "api/materials"));
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrWhiteSpace(materialName))
        {
            query["materialName"] = materialName;
        }
        if (!string.IsNullOrWhiteSpace(materialType))
        {
            query["materialType"] = materialType;
        }
        if (isActive.HasValue)
        {
            query["isActive"] = isActive.Value.ToString().ToLowerInvariant();
        }
        if (warehouseId.HasValue)
        {
            query["warehouseId"] = warehouseId.Value.ToString();
        }

        var queryString = query.ToString();
        if (!string.IsNullOrEmpty(queryString))
        {
            builder.Query = queryString;
        }

        return await _httpClient.GetFromJsonAsync<List<MaterialListItemResponse>>(builder.Uri);
    }

    public async Task<MaterialDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<MaterialDetailsResponse>($"api/materials/{id}");
    }

    public async Task UpdateAsync(Guid id, UpdateMaterialRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/materials/{id}", request);
        response.EnsureSuccessStatusCode();
    }
}
