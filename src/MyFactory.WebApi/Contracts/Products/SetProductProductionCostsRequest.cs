namespace MyFactory.WebApi.Contracts.Products;

public record SetProductProductionCostsRequest(IReadOnlyCollection<ProductDepartmentCostRequest> Costs);
