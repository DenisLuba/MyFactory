namespace MyFactory.MauiClient.Models.ProductionOrders;

public record IssueMaterialsToProductionRequest(IReadOnlyList<IssueMaterialLineRequest> Materials);
