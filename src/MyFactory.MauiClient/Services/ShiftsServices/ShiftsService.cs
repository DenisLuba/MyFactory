using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Shifts;

namespace MyFactory.MauiClient.Services.ShiftsServices
{
    public class ShiftsService(HttpClient httpClient) : IShiftsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ShiftsCreatePlanResponse?> CreatePlanAsync(ShiftsCreatePlanRequest request)
            => await _httpClient.PostAsJsonAsync("api/shifts/plans", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ShiftsCreatePlanResponse>()).Unwrap();

        public async Task<List<ShiftsGetPlansResponse>?> GetPlansAsync(DateTime? date = null)
            => await _httpClient.GetFromJsonAsync<List<ShiftsGetPlansResponse>>($"api/shifts/plans{(date.HasValue ? "?date=" + date.Value.ToString("yyyy-MM-dd") : "")}");

        public async Task<ShiftsRecordResultResponse?> RecordResultAsync(ShiftsRecordResultRequest request)
            => await _httpClient.PostAsJsonAsync("api/shifts/results", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ShiftsRecordResultResponse>()).Unwrap();

        public async Task<List<ShiftsGetResultsResponse>?> GetResultsAsync(Guid? employeeId = null, DateTime? date = null)
        {
            var query = "";
            if (employeeId.HasValue) query += $"employeeId={employeeId.Value}&";
            if (date.HasValue) query += $"date={date.Value:yyyy-MM-dd}&";
            if (!string.IsNullOrEmpty(query)) query = "?" + query.TrimEnd('&');
            return await _httpClient.GetFromJsonAsync<List<ShiftsGetResultsResponse>>($"api/shifts/results{query}");
        }
    }
}