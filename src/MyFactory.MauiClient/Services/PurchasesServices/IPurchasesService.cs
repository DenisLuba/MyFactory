using MyFactory.MauiClient.Models.Purchases;

namespace MyFactory.MauiClient.Services.PurchasesServices
{
    public interface IPurchasesService
    {
        Task<PurchasesCreateResponse?> CreatePurchaseAsync(PurchasesCreateRequest request);
        Task<List<PurchasesResponse>?> PurchasesListAsync();
        Task<PurchasesConvertToOrderResponse?> ConvertToOrderAsync(string id);
    }
}