namespace MyFactory.WebApi.Contracts.MaterialTransfers;

public record MaterialTransferItemRequest(
    Guid MaterialId,
    string MaterialName,
    decimal Quantity,
    string Unit,
    decimal Price);
