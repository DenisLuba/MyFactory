using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Services.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MyFactory.MauiClient.Services.Materials;

public sealed class MaterialsService : IMaterialsService
{
    private readonly HttpClient _httpClient;

    public MaterialsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ListResponse<MaterialListItemResponse>?> GetListAsync(
        string? searchName =null,
        string? searchType = null,
        string? sortBy = null,
        bool sortDesc = false,
        int skip = 0,
        int take = 30,
        bool? isActive = true,
        Guid? warehouseId = null)
    {
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(searchName))
            query.Add($"searchName={Uri.EscapeDataString(searchName)}");
        if (!string.IsNullOrWhiteSpace(searchType))
            query.Add($"searchType={Uri.EscapeDataString(searchType)}");
        if (!string.IsNullOrWhiteSpace(sortBy))
            query.Add($"sortBy={Uri.EscapeDataString(sortBy)}");
        query.Add($"sortDesc={sortDesc.ToString().ToLowerInvariant()}");
        query.Add($"skip={skip}");
        query.Add($"take={take}");
        if (isActive.HasValue)
            query.Add($"isActive={isActive.Value.ToString().ToLowerInvariant()}");
        if (warehouseId.HasValue)
            query.Add($"warehouseId={warehouseId.Value}");

        var path = "/api/materials" + (query.Count > 0 ? $"?{string.Join("&", query)}" : string.Empty);
        return await _httpClient.GetFromJsonAsync<ListResponse<MaterialListItemResponse>?>(path);
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

    // IMAGES

    public async Task<IReadOnlyList<MaterialImageFileResponse>?> GetImagesAsync(Guid MaterialId)
    {
        return await _httpClient.GetFromJsonAsync<List<MaterialImageFileResponse>>($"api/materials/{MaterialId}/images");
    }

    public async Task<byte[]?> GetImageContentAsync(Guid imageId, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync($"api/materials/images/{imageId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    public async Task<Guid?> UploadImageAsync(Guid MaterialId, Stream content, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        using var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
        using var form = new MultipartFormDataContent
        {
            { streamContent, "file", fileName }
        };

        using var response = await _httpClient.PostAsync($"api/materials/{MaterialId}/images", form, cancellationToken);
        await response.EnsureSuccessWithProblemAsync(cancellationToken);
        return await response.Content.ReadFromJsonAsync<Guid>(cancellationToken);
    }

    public async Task DeleteImageAsync(Guid imageId)
    {
        var response = await _httpClient.DeleteAsync($"api/materials/images/{imageId}");
        await response.EnsureSuccessWithProblemAsync();
    }
}
