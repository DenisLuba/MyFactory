namespace MyFactory.MauiClient.Models.Production;

public record ProductionCreateOrderRequest(
    Guid SpecificationId,
    int Quantity);
