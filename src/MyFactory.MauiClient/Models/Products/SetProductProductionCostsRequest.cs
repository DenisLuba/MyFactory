namespace MyFactory.MauiClient.Models.Products;

public record SetProductProductionCostsRequest(IReadOnlyCollection<ProductDepartmentCostRequest> Costs);
