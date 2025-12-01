namespace MyFactory.WebApi.Contracts.MaterialTransfers;

public record MaterialTransferCreateResponse(
    Guid TransferId,
    MaterialTransferStatus Status);
