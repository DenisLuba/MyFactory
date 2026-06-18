using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Units;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Units;

public sealed class UnitsService : IUnitsService
{
    private readonly HttpClient _httpClient;
    private const string BaseRoute = "api/units";

    public UnitsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<UnitResponse>?> GetListAsync(CancellationToken ct = default)
    {
        return await _httpClient.GetFromJsonAsync<IReadOnlyList<UnitResponse>>(BaseRoute, ct);
    }

    public async Task<Guid> CreateAsync(AddUnitRequest request, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, ct);
        await response.EnsureSuccessWithProblemAsync(ct);

        var body = await response.Content.ReadFromJsonAsync<AddUnitResponse>(cancellationToken: ct);
        return body?.Id ?? throw new InvalidOperationException("Invalid create unit response");
    }

    public async Task UpdateAsync(Guid id, UpdateUnitRequest request, CancellationToken ct = default)
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
