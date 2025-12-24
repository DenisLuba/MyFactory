namespace MyFactory.Application.DTOs.Products;

public sealed record ProductDepartmentCostDto
{
    public Guid DepartmentId { get; init; }
    public string DepartmentName { get; init; } = default!;

    public decimal CutCost { get; init; }
    public decimal SewingCost { get; init; }
    public decimal PackCost { get; init; }
    public decimal Expenses { get; init; }

    public decimal Total =>
        CutCost + SewingCost + PackCost + Expenses;
}