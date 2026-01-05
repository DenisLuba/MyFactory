using System.Globalization;
using System.Net.Http.Json;
using System.Web;
using MyFactory.MauiClient.Models.Finance;

namespace MyFactory.MauiClient.Services.Finance;

public sealed class FinanceService : IFinanceService
{
    private readonly HttpClient _httpClient;

    public FinanceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<PayrollAccrualListItemResponse>?> GetPayrollAccrualsAsync(DateOnly from, DateOnly to, Guid? employeeId = null, Guid? departmentId = null)
    {
        var builder = new UriBuilder(new Uri(_httpClient.BaseAddress!, "api/finance/payroll/accruals"));
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        query["to"] = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (employeeId.HasValue)
        {
            query["employeeId"] = employeeId.Value.ToString();
        }

        if (departmentId.HasValue)
        {
            query["departmentId"] = departmentId.Value.ToString();
        }

        builder.Query = query.ToString();
        return await _httpClient.GetFromJsonAsync<List<PayrollAccrualListItemResponse>>(builder.Uri);
    }

    public async Task<EmployeePayrollAccrualDetailsResponse?> GetEmployeePayrollAccrualsAsync(Guid employeeId, int year, int month)
    {
        var builder = new UriBuilder(new Uri(_httpClient.BaseAddress!, $"api/finance/payroll/employees/{employeeId}/accruals"));
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["year"] = year.ToString(CultureInfo.InvariantCulture);
        query["month"] = month.ToString(CultureInfo.InvariantCulture);
        builder.Query = query.ToString();
        return await _httpClient.GetFromJsonAsync<EmployeePayrollAccrualDetailsResponse>(builder.Uri);
    }

    public async Task CalculateDailyAccrualAsync(CalculateDailyPayrollAccrualRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/finance/payroll/accruals/calculate/daily", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task CalculatePeriodAccrualsAsync(CalculatePayrollAccrualsForPeriodRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/finance/payroll/accruals/calculate/period", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task AdjustAccrualAsync(Guid accrualId, AdjustPayrollAccrualRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/finance/payroll/accruals/{accrualId}/adjust", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<CreatePayrollPaymentResponse?> CreatePayrollPaymentAsync(CreatePayrollPaymentRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/finance/payroll/payments", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CreatePayrollPaymentResponse>();
    }
}
