using System.Net.Http.Json;
using MyFactory.MauiClient.Models.Returns;

namespace MyFactory.MauiClient.Services.ReturnsServices
{
    public class ReturnsService(HttpClient httpClient) : IReturnsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ReturnsCreateResponse?> CreateReturnAsync(ReturnsCreateRequest request)
            => await _httpClient.PostAsJsonAsync("api/returns", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ReturnsCreateResponse>()).Unwrap();
    }
}