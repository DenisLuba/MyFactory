namespace MyFactory.MauiClient.Models.Products;

public record ProductDepartmentCostRequest(
    Guid DepartmentId,
    decimal CutCost,
    decimal SewingCost,
    decimal PackCost,
    decimal Expenses);
