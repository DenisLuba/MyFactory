using MyFactory.MauiClient.Models.Shipments;

namespace MyFactory.MauiClient.Services.ShipmentsServices
{
    public interface IShipmentsService
    {
        Task<ShipmentsCreateResponse?> CreateShipmentAsync(ShipmentsCreateRequest request);
        Task<ShipmentsGetResponse?> GetAsync(Guid id);
        Task<ShipmentsConfirmPaymentResponse?> ConfirmPaymentAsync(Guid id);
    }
}