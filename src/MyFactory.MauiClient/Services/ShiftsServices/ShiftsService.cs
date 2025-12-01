using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Shifts;

namespace MyFactory.MauiClient.Services.ShiftsServices
{
    public class ShiftsService(HttpClient httpClient) : IShiftsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<IReadOnlyList<ShiftResultListResponse>?> GetResultsAsync(Guid? employeeId = null, DateTime? date = null)
        {
            var queryParts = new List<string>();
            if (employeeId.HasValue)
            {
                queryParts.Add($"employeeId={employeeId.Value}");
            }

            if (date.HasValue)
            {
                queryParts.Add($"date={date.Value:yyyy-MM-dd}");
            }

            var query = queryParts.Count > 0 ? "?" + string.Join("&", queryParts) : string.Empty;
            return await _httpClient.GetFromJsonAsync<List<ShiftResultListResponse>>($"api/shifts/results{query}");
        }

        public async Task<ShiftResultCardResponse?> GetResultByIdAsync(Guid shiftPlanId)
            => await _httpClient.GetFromJsonAsync<ShiftResultCardResponse>($"api/shifts/results/{shiftPlanId}");

        public async Task<ShiftsRecordResultResponse?> SaveResultAsync(ShiftsRecordResultRequest request)
            => await _httpClient.PostAsJsonAsync("api/shifts/results", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ShiftsRecordResultResponse>()).Unwrap();
    }
}