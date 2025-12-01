namespace MyFactory.WebApi.Contracts.MaterialTransfers;

public record MaterialTransferItemDto(
    Guid MaterialId,
    string MaterialName,
    decimal Quantity,
    string Unit,
    decimal Price,
    decimal LineTotal);
