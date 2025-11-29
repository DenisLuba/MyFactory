using MyFactory.MauiClient.Models.FinishedGoods;

namespace MyFactory.MauiClient.Services.FinishedGoodsServices
{
    public interface IFinishedGoodsService
    {
        Task<ReceiptFinishedGoodsResponse?> ReceiptAsync(ReceiptFinishedGoodsRequest request);
        Task<List<FinishedGoodsInventoryResponse>?> GetInventoryAsync();
        Task<MoveFinishedGoodsResponse?> MoveAsync(MoveFinishedGoodsRequest request);
    }
}