namespace MyFactory.MauiClient.Models.Production.MaterialTransfers;

public record MaterialTransferSubmitResponse(
    Guid TransferId,
    MaterialTransferStatus Status,
    DateTime SubmittedAt);
