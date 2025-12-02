using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Payroll;

namespace MyFactory.MauiClient.Services.PayrollServices
{
    public class PayrollService(HttpClient httpClient) : IPayrollService
    {
        private readonly HttpClient _httpClient = httpClient;

        /*public async Task<List<PayrollGetResponse>?> GetAsync(int periodMonth, int periodYear)
            => await _httpClient.GetFromJsonAsync<List<PayrollGetResponse>>($"api/payroll?periodMonth={periodMonth}&periodYear={periodYear}");

        public async Task<PayrollCalculateResponse?> CalculateAsync(DateTime from, DateTime to)
            => await _httpClient.PostAsync($"api/payroll/calc?from={from:O}&to={to:O}", null)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<PayrollCalculateResponse>()).Unwrap();

        public async Task<PayrollPayResponse?> PayAsync(PayrollPayRequest request)
            => await _httpClient.PostAsJsonAsync("api/payroll/pay", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<PayrollPayResponse>()).Unwrap();*/
    }
}