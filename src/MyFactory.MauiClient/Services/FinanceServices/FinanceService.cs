using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.UIModels.Finance;

namespace MyFactory.MauiClient.Services.FinanceServices
{
    public class FinanceService : IFinanceService
    {
        private readonly HttpClient _httpClient;
        public FinanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RecordOverheadResponse?> AddOverheadAsync(RecordOverheadRequest request)
            => await _httpClient.PostAsJsonAsync("api/finance/overheads", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<RecordOverheadResponse>()).Unwrap();

        public async Task<RecordOverheadResponse?> UpdateOverheadAsync(Guid overheadId, RecordOverheadRequest request)
            => await _httpClient.PutAsJsonAsync($"api/finance/overheads/{overheadId}", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<RecordOverheadResponse>()).Unwrap();

        public async Task<RecordOverheadResponse?> PostOverheadAsync(Guid overheadId)
            => await _httpClient.PutAsync($"api/finance/overheads/{overheadId}/post", null)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<RecordOverheadResponse>()).Unwrap();

        public async Task<RecordOverheadResponse?> DeleteOverheadAsync(Guid overheadId)
            => await _httpClient.DeleteAsync($"api/finance/overheads/{overheadId}")
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<RecordOverheadResponse>()).Unwrap();

        public async Task<List<OverheadItem>?> GetOverheadsAsync(int month, int year, string? article = null, OverheadStatus? status = null)
        {
            var query = new List<string>
            {
                $"month={month}",
                $"year={year}"
            };

            if (!string.IsNullOrWhiteSpace(article))
            {
                query.Add($"article={Uri.EscapeDataString(article)}");
            }

            if (status.HasValue)
            {
                query.Add($"status={status.Value}");
            }

            var url = $"api/finance/overheads?{string.Join("&", query)}";
            return await _httpClient.GetFromJsonAsync<List<OverheadItem>>(url);
        }

        public async Task<List<string>?> GetOverheadArticlesAsync()
            => await _httpClient.GetFromJsonAsync<List<string>>("api/finance/overheads/articles");

        /*public async Task<AdvanceStatusResponse?> CreateAdvanceAsync(CreateAdvanceRequest request)
            => await _httpClient.PostAsJsonAsync("api/finance/advances", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<AdvanceStatusResponse>()).Unwrap();*/

        public async Task<AdvanceStatusResponse?> SubmitAdvanceReportAsync(string advanceId, SubmitAdvanceReportRequest request)
            => await _httpClient.PostAsJsonAsync($"api/finance/advances/{advanceId}/report", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<AdvanceStatusResponse>()).Unwrap();

        public async Task<List<AdvanceItem>?> GetAdvancesAsync()
            => await _httpClient.GetFromJsonAsync<List<AdvanceItem>>("api/finance/advances");

        public async Task<AdvanceStatusResponse?> CloseAdvanceAsync(string advanceNumber)
            => await _httpClient.PutAsync($"api/finance/advances/{advanceNumber}/close", null)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<AdvanceStatusResponse>()).Unwrap();

        public async Task<AdvanceStatusResponse?> DeleteAdvanceAsync(string advanceNumber)
            => await _httpClient.DeleteAsync($"api/finance/advances/{advanceNumber}")
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<AdvanceStatusResponse>()).Unwrap();
    }
}