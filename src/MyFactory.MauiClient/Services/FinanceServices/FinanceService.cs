using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.UIModels.Finance;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<List<OverheadResponse>?> GetOverheadsAsync(int month, int year)
            => await _httpClient.GetFromJsonAsync<List<OverheadResponse>>($"api/finance/overheads?month={month}&year={year}");

        public async Task<CreateAdvanceResponse?> CreateAdvanceAsync(CreateAdvanceRequest request)
            => await _httpClient.PostAsJsonAsync("api/finance/advances", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<CreateAdvanceResponse>()).Unwrap();

        public async Task<SubmitAdvanceReportResponse?> SubmitAdvanceReportAsync(string advanceId, SubmitAdvanceReportRequest request)
            => await _httpClient.PostAsJsonAsync($"api/finance/advances/{advanceId}/report", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<SubmitAdvanceReportResponse>()).Unwrap();

        public async Task<List<AdvanceItem>?> GetAdvancesAsync()
            => await _httpClient.GetFromJsonAsync<List<AdvanceItem>>("api/finance/advances");

        public async Task CloseAdvanceAsync(string advanceNumber)
            => await _httpClient.PostAsync($"api/finance/advances/{advanceNumber}/close", null);

        public async Task DeleteAdvanceAsync(string advanceNumber)
            => await _httpClient.DeleteAsync($"api/finance/advances/{advanceNumber}");
    }
}