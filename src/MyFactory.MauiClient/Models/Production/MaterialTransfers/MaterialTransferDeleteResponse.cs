namespace MyFactory.MauiClient.Models.Production.MaterialTransfers;

public record MaterialTransferDeleteResponse(
    Guid TransferId,
    bool IsDeleted);
