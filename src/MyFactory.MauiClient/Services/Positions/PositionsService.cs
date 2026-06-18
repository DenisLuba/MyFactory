using System.Net.Http.Json;
using System.Web;
using MyFactory.MauiClient.Models.Positions;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Positions;

public sealed class PositionsService : IPositionsService
{
    private readonly HttpClient _httpClient;

    public PositionsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<PositionListItemResponse>?> GetListAsync(Guid? departmentId = null, bool includeInactive = false, PositionSortBy sortBy = PositionSortBy.Name, bool sortDesc = false)
    {
        var builder = new UriBuilder(new Uri(_httpClient.BaseAddress!, "api/positions"));
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (departmentId.HasValue)
        {
            query["departmentId"] = departmentId.Value.ToString();
        }
        if (includeInactive)
        {
            query["includeInactive"] = includeInactive.ToString().ToLowerInvariant();
        }
        if (sortBy != PositionSortBy.Name)
        {
            query["sortBy"] = sortBy.ToString();
        }
        if (sortDesc)
        {
            query["sortDesc"] = sortDesc.ToString().ToLowerInvariant();
        }

        var queryString = query.ToString();
        if (!string.IsNullOrEmpty(queryString))
        {
            builder.Query = queryString;
        }

        return await _httpClient.GetFromJsonAsync<List<PositionListItemResponse>>(builder.Uri);
    }

    public async Task<PositionDetailsResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<PositionDetailsResponse>($"api/positions/{id}");
    }

    public async Task<CreatePositionResponse?> CreateAsync(CreatePositionRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/positions", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreatePositionResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdatePositionRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/positions/{id}", request);
        await response.EnsureSuccessWithProblemAsync();
    }
}
