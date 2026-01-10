using System.Net.Http.Json;
using MyFactory.MauiClient.Models.PayrollRules;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.PayrollRules;

public sealed class PayrollRulesService : IPayrollRulesService
{
    private readonly HttpClient _httpClient;

    public PayrollRulesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<PayrollRuleResponse>?> GetListAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<PayrollRuleResponse>>("api/payroll-rules");
    }

    public async Task<PayrollRuleResponse?> GetDetailsAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<PayrollRuleResponse>($"api/payroll-rules/{id}");
    }

    public async Task<CreatePayrollRuleResponse?> CreateAsync(CreatePayrollRuleRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/payroll-rules", request);
        await response.EnsureSuccessWithProblemAsync();
        return await response.Content.ReadFromJsonAsync<CreatePayrollRuleResponse>();
    }

    public async Task UpdateAsync(Guid id, UpdatePayrollRuleRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/payroll-rules/{id}", request);
        await response.EnsureSuccessWithProblemAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/payroll-rules/{id}");
        await response.EnsureSuccessWithProblemAsync();
    }
}
