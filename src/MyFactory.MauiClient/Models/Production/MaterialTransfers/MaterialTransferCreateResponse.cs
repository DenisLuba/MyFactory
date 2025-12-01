namespace MyFactory.MauiClient.Models.Production.MaterialTransfers;

public record MaterialTransferCreateResponse(
    Guid TransferId,
    MaterialTransferStatus Status);
