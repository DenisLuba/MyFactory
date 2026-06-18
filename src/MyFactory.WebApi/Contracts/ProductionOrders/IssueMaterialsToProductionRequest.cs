namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record IssueMaterialsToProductionRequest(IReadOnlyList<IssueMaterialLineRequest> Materials);
