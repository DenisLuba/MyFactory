using MyFactory.MauiClient.Models.Shipments;

namespace MyFactory.MauiClient.Services.Shipments;

public interface IShipmentsService
{
    Task<IReadOnlyList<ShipmentListItemResponse>?> GetListAsync(
        Guid? salesOrderId = null,
        Guid? customerId = null,
        ShipmentStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken ct = default);

    Task<ShipmentDetailsResponse?> GetDetailsAsync(Guid id, CancellationToken ct = default);

    Task<Guid> CreateAsync(CreateShipmentRequest request, CancellationToken ct = default);

    Task UpdateAsync(Guid id, UpdateShipmentRequest request, CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
