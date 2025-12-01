namespace MyFactory.WebApi.Contracts.MaterialTransfers;

public record MaterialTransferSubmitResponse(
    Guid TransferId,
    MaterialTransferStatus Status,
    DateTime SubmittedAt);
