using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Returns;

namespace MyFactory.MauiClient.Services.ReturnsServices
{
    public class ReturnsService(HttpClient httpClient) : IReturnsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<IReadOnlyList<ReturnsListResponse>> GetReturnsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<List<ReturnsListResponse>>("api/returns", cancellationToken);
            return response ?? new List<ReturnsListResponse>();
        }

        public Task<ReturnCardResponse?> GetReturnAsync(Guid returnId, CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<ReturnCardResponse>($"api/returns/{returnId}", cancellationToken);

        public async Task<ReturnsCreateResponse?> CreateReturnAsync(ReturnsCreateRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("api/returns", request, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReturnsCreateResponse>(cancellationToken);
        }
    }
}