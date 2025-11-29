using System.Net.Http.Json;
using MyFactory.MauiClient.Models.FinishedGoods;

namespace MyFactory.MauiClient.Services.FinishedGoodsServices
{
    public class FinishedGoodsService(HttpClient httpClient) : IFinishedGoodsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ReceiptFinishedGoodsResponse?> ReceiptAsync(ReceiptFinishedGoodsRequest request)
            => await _httpClient.PostAsJsonAsync("api/finished-goods/receipt", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<ReceiptFinishedGoodsResponse>()).Unwrap();

        public async Task<List<FinishedGoodsInventoryResponse>?> GetInventoryAsync()
            => await _httpClient.GetFromJsonAsync<List<FinishedGoodsInventoryResponse>>("api/finished-goods");

        public async Task<MoveFinishedGoodsResponse?> MoveAsync(MoveFinishedGoodsRequest request)
            => await _httpClient.PostAsJsonAsync("api/finished-goods/move", request)
                .ContinueWith(t => t.Result.Content.ReadFromJsonAsync<MoveFinishedGoodsResponse>()).Unwrap();
    }
}