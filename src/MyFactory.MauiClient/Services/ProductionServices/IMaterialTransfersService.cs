using MyFactory.MauiClient.Models.Production.MaterialTransfers;

namespace MyFactory.MauiClient.Services.ProductionServices;

public interface IMaterialTransfersService
{
    Task<IReadOnlyList<MaterialTransferListResponse>?> GetTransfersAsync(DateTime? dateFilter = null, string? warehouse = null, string? productionOrder = null);
    Task<MaterialTransferCardResponse?> GetByIdAsync(Guid transferId);
    // Task<MaterialTransferCreateResponse?> CreateAsync(MaterialTransferCreateRequest request);
    // Task<MaterialTransferUpdateResponse?> UpdateAsync(Guid transferId, MaterialTransferUpdateRequest request);
    // Task<MaterialTransferDeleteResponse?> DeleteAsync(Guid transferId);
    Task<MaterialTransferSubmitResponse?> SubmitAsync(Guid transferId);
}
