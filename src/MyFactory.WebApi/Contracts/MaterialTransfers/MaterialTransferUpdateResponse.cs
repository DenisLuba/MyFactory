namespace MyFactory.WebApi.Contracts.MaterialTransfers;

public record MaterialTransferUpdateResponse(
    Guid TransferId,
    MaterialTransferStatus Status);
