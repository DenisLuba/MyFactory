namespace MyFactory.MauiClient.Models.Production.MaterialTransfers;

public record MaterialTransferItemRequest(
    Guid MaterialId,
    string MaterialName,
    decimal Quantity,
    string Unit,
    decimal Price);
