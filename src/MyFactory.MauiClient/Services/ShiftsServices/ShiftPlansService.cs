using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Shifts;

namespace MyFactory.MauiClient.Services.ShiftsServices
{
    public class ShiftPlansService(HttpClient httpClient) : IShiftPlansService
    {
        private readonly HttpClient _httpClient = httpClient;

        /*public async Task<ShiftsCreatePlanResponse?> CreatePlanAsync(ShiftsCreatePlanRequest request)
            => await _httpClient.PostAsJsonAsync("api/shifts/plans", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ShiftsCreatePlanResponse>()).Unwrap();*/

        public async Task<IReadOnlyList<ShiftPlanListResponse>?> GetPlansAsync(DateTime? date = null)
        {
            var query = date.HasValue ? $"?date={date.Value:yyyy-MM-dd}" : string.Empty;
            return await _httpClient.GetFromJsonAsync<List<ShiftPlanListResponse>>($"api/shifts/plans{query}");
        }

        public async Task<ShiftPlanCardResponse?> GetPlanByIdAsync(Guid shiftPlanId)
            => await _httpClient.GetFromJsonAsync<ShiftPlanCardResponse>($"api/shifts/plans/{shiftPlanId}");

        /*public async Task<ShiftsRecordResultResponse?> RecordResultAsync(ShiftsRecordResultRequest request)
            => await _httpClient.PostAsJsonAsync("api/shifts/results", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ShiftsRecordResultResponse>()).Unwrap();*/
    }
}
