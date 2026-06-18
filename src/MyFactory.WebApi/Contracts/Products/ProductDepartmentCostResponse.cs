namespace MyFactory.WebApi.Contracts.Products;

public record ProductDepartmentCostResponse(
    Guid DepartmentId,
    string DepartmentName,
    decimal CutCost,
    decimal SewingCost,
    decimal PackCost,
    decimal Expenses,
    decimal Total);
