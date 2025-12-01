namespace MyFactory.MauiClient.Models.Production.MaterialTransfers;

public record MaterialTransferUpdateResponse(
    Guid TransferId,
    MaterialTransferStatus Status);
