namespace MyFactory.WebApi.Contracts.Products;

public record ProductDepartmentCostRequest(
    Guid DepartmentId,
    decimal CutCost,
    decimal SewingCost,
    decimal PackCost,
    decimal Expenses);
