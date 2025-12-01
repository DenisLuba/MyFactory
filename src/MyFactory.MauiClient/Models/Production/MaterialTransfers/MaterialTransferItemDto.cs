namespace MyFactory.MauiClient.Models.Production.MaterialTransfers;

public record MaterialTransferItemDto(
    Guid MaterialId,
    string MaterialName,
    decimal Quantity,
    string Unit,
    decimal Price,
    decimal LineTotal);
