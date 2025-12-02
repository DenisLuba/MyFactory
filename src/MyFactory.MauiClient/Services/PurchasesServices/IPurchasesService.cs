using MyFactory.MauiClient.Models.Purchases;

namespace MyFactory.MauiClient.Services.PurchasesServices
{
    public interface IPurchasesService
    {
        Task<List<PurchasesResponse>?> PurchasesListAsync();
        Task<PurchaseRequestDetailResponse?> GetPurchaseRequestAsync(Guid id);
        Task<PurchasesCreateResponse?> CreatePurchaseAsync(PurchasesCreateRequest request);
        Task<PurchasesCreateResponse?> UpdatePurchaseAsync(Guid id, PurchasesCreateRequest request);
        Task DeletePurchaseAsync(Guid id);
        Task<PurchasesConvertToOrderResponse?> ConvertToOrderAsync(Guid id);
    }
}