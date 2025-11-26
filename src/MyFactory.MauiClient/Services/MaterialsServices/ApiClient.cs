using MyFactory.MauiClient.Models;
using System.Net.Http.Json;

namespace MyFactory.MauiClient.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;

    public ApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<IEnumerable<MaterialListItem>> GetMaterialsAsync()
    {
        return await _http.GetFromJsonAsync<IEnumerable<MaterialListItem>>(
            "https://localhost:5001/api/materials"
        ) ?? [];
    }
}


