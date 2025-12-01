namespace MyFactory.WebApi.Contracts.MaterialTransfers;

public record MaterialTransferDeleteResponse(
    Guid TransferId,
    bool IsDeleted);
