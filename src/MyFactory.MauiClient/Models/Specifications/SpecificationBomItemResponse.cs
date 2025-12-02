namespace MyFactory.MauiClient.Models.Specifications;

public record SpecificationBomItemResponse(
    Guid Id,
    string Material,
    double Quantity,
    string Unit,
    decimal Price,
    decimal Cost
);
