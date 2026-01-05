using System.Globalization;
using System.Net.Http.Json;
using System.Web;
using MyFactory.MauiClient.Models.Expences;

namespace MyFactory.MauiClient.Services.Expences;

public sealed class ExpencesService : IExpencesService
{
    private readonly HttpClient _httpClient;

    public ExpencesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ExpenseListItemResponse>?> GetListAsync(DateOnly from, DateOnly to, Guid? expenseTypeId = null)
    {
        var builder = new UriBuilder(new Uri(_httpClient.BaseAddress!, "api/expences"));
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        query["to"] = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (expenseTypeId.HasValue)
        {
            query["expenseTypeId"] = expenseTypeId.Value.ToString();
        }
        builder.Query = query.ToString();
        return await _httpClient.GetFromJsonAsync<List<ExpenseListItemResponse>>(builder.Uri);
    }

    public async Task<CreateExpenseResponse?> CreateAsync(CreateExpenseRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/expences", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreateExpenseResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdateExpenseRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/expences/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/expences/{id}");
        response.EnsureSuccessStatusCode();
    }
}
